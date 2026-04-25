.code
PixelizeProc proc
    ; --- ARGUMENTY PRZEKAZYWANE PRZEZ C# (Konwencja x64) ---
    ; RCX = wskaznik na dane obrazu (scan0)
    ; RDX = szerokosc obrazu (Width) -> UWAGA: DIV niszczy RDX!
    ; R8  = wysokosc obrazu (Height)
    ; R9  = stride (szerokosc wiersza w bajtach)
    ; [RBP+48] = wielkosc bloku (blockSize) - na stosie

    push rbp
    mov rbp, rsp
    
    ; --- ZAPAMIETYWANIE REJESTROW (Wymagane przez Windows x64) ---
    push rbx
    push rsi
    push rdi        ; RDI bedzie trzymal szerokosc
    push r12
    push r13
    push r14
    push r15

    ; --- PRZYGOTOWANIE ZMIENNYCH ---
    mov rdi, rdx    ; Przenosze Width do RDI, bo instrukcja DIV zniszczy RDX
    mov rbx, r8     ; Przenosze Height do RBX dla wygody

    mov r10, [rbp + 48]     ; Pobierz BlockSize ze stosu
    cmp r10, 1
    jle EndProc             ; Jak blok <= 1 to nie ma co robic, koniec

    xor r11, r11            ; r11 to nasza petla Y (CurrentY = 0)

LoopY:
    cmp r11, rbx            ; Czy koniec wysokosci?
    jge EndProc

    xor r12, r12            ; r12 to nasza petla X (CurrentX = 0)

LoopX:
    cmp r12, rdi            ; Czy koniec szerokosci? (Porownujemy z RDI!)
    jge NextBlockY

    ; --- ZEROWANIE SUMATOROW ---
    pxor xmm5, xmm5         ; Wyzeruj rejestr SSE na sumy kolorow
    xor r13, r13            ; r13 = licznik pikseli w bloku (Count)

    ; --- OBLICZANIE ADRESU STARTOWEGO BLOKU ---
    ; Adres = Scan0 + (Y * Stride) + (X * 4)
    mov rax, r11
    imul rax, r9            ; Y * Stride
    mov r8, r12             
    shl r8, 2               ; X * 4 (bo 4 bajty na piksel)
    add rax, r8
    add rax, rcx            ; dodaj adres bazowy
    mov rsi, rax            ; RSI wskazuje teraz na lewy-gorny rog bloku

    ; --- ZBIERANIE KOLOROW (Srednia) ---
    xor r14, r14            ; Wewnetrzne Y w bloku
InnerLoopY:
    cmp r14, r10            ; Czy koniec bloku w pionie?
    jge CalcAvg
    mov rax, r11
    add rax, r14
    cmp rax, rbx            ; Sprawdz czy nie wychodzimy poza obraz (dol)
    jge CalcAvg

    xor r15, r15            ; Wewnetrzne X w bloku
InnerLoopX:
    cmp r15, r10            ; Czy koniec bloku w poziomie?
    jge NextInY
    mov rax, r12
    add rax, r15
    cmp rax, rdi            ; Sprawdz czy nie wychodzimy poza obraz (prawo)
    jge NextInY

    ; Pobierz piksel i dodaj do sumy
    movd xmm0, dword ptr [rsi + r15*4]  ; Wczytaj 4 bajty (BGRA)
    pmovzxbw xmm0, xmm0                 ; Rozszerz bajty na slowa (Words)
    pmovzxwd xmm0, xmm0                 ; Rozszerz slowa na inty (Dwords)
    paddd xmm5, xmm0                    ; Dodaj do sumy w XMM5
    
    inc r13                 ; Zwieksz licznik pikseli
    inc r15                 ; Nastepny X w bloku
    jmp InnerLoopX

NextInY:
    add rsi, r9             ; Przesun wskaznik o caly wiersz w dol
    inc r14                 ; Nastepny Y w bloku
    jmp InnerLoopY

CalcAvg:
    cmp r13, 0              ; Zabezpieczenie przed dzieleniem przez 0
    je NextBlockX

    ; --- OBLICZANIE SREDNIEJ ---
    ; Dzielimy sumy przez ilosc pikseli (r13)
    ; Uzywamy RDX:RAX do dzielenia, dlatego wczesniej schowalismy Width
    
    ; -- Niebieski (Blue) --
    movd eax, xmm5          ; Pobierz sume Blue
    xor rdx, rdx            ; Wyzeruj gorna czesc przed dzieleniem
    div r13d                ; Dzielenie: EAX / Count
    mov r8d, eax            ; Wynik do r8d

    ; -- Zielony (Green) --
    pextrd eax, xmm5, 1     ; Pobierz sume Green
    xor rdx, rdx
    div r13d
    shl eax, 8              ; Przesun na pozycje Green
    or r8d, eax             ; Polacz z wynikiem

    ; -- Czerwony (Red) --
    pextrd eax, xmm5, 2     ; Pobierz sume Red
    xor rdx, rdx
    div r13d
    shl eax, 16             ; Przesun na pozycje Red
    or r8d, eax             ; Polacz z wynikiem
    
    or r8d, 0FF000000h      ; Ustaw Alpha na 255 (pelna widocznosc)

    ; --- MALOWANIE BLOKU ---
    ; Musimy znowu obliczyc adres startowy, bo RSI sie zmienilo
    mov rax, r11
    imul rax, r9
    mov rdx, r12            ; RDX jest teraz wolny, mozna uzyc do obliczen
    shl rdx, 2
    add rax, rdx
    add rax, rcx
    mov rsi, rax            ; RSI znowu na poczatku bloku

    xor r14, r14
FillY:
    cmp r14, r10
    jge NextBlockX
    mov rax, r11
    add rax, r14
    cmp rax, rbx            ; Sprawdz granice Y
    jge NextBlockX

    xor r15, r15
FillX:
    cmp r15, r10
    jge NextFillY
    mov rax, r12
    add rax, r15
    cmp rax, rdi            ; Sprawdz granice X (uzywamy RDI!)
    jge NextFillY

    mov [rsi + r15*4], r8d  ; Zapisz obliczony kolor (Srednia)
    inc r15
    jmp FillX

NextFillY:
    add rsi, r9             ; Nastepny wiersz
    inc r14
    jmp FillY

    ; --- NASTEPNE BLOKI ---
NextBlockX:
    add r12, r10            ; Przesun X o wielkosc bloku
    jmp LoopX

NextBlockY:
    add r11, r10            ; Przesun Y o wielkosc bloku
    jmp LoopY

EndProc:
    ; --- PRZYWRACANIE REJESTROW ---
    pop r15
    pop r14
    pop r13
    pop r12
    pop rdi
    pop rsi
    pop rbx
    pop rbp
    ret
PixelizeProc endp
end