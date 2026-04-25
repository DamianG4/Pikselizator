# Pikselizator (C# + ASM x64)

Aplikacja okienkowa służąca do nakładania filtru pikselizacji na obraz. Projekt łączy interfejs napisany w języku C# (WinForms) z wysokowydajną biblioteką obliczeniową napisaną w Asemblerze x64.

## Technologie
* **Interfejs:** C# / .NET Framework
* **Obliczenia:** Asembler x64 (MASM) z wykorzystaniem instrukcji wektorowych SIMD
* **Optymalizacje:** Wielowątkowość.

## Instrukcja uruchomienia

### Gotowa aplikacja
Najszybszy sposób na uruchomienie to pobranie skompilowanej paczki z zakładki **Releases**. Wypakuj archiwum i uruchom plik `Pikselizator.exe`.

### Samodzielna kompilacja
1. Wymagane jest środowisko **Visual Studio 2022** z doinstalowanym pakietem **Programowanie aplikacji klasycznych w języku C++** (narzędzia kompilacji v143 dla MASM).
2. Otwórz plik rozwiązania `Pikselizator.sln`.
3. Skompiluj najpierw projekt `AsmLibrary`.
4. Skompiluj projekt główny `Pikselizator`
5. Upewnij się, że plik `AsmLibrary.dll` znajduje się w tym samym folderze wyjściowym, co wygenerowany plik `Pikselizator.exe`.
