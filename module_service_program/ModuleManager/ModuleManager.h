#include <string>
#include <vector>

using namespace std;

/*
 * Klasa do modyfikacji dla prowadzacych laboratorium. 
 * Pola, ktorych wartosci mozna ustawic:
   - sciezka lub sciezki do folderow ktore beda skanowane przez serwis modulowy -> scan_folder_paths
   - pelne nazwy plikow/przedroski/przyrostki nazw plikow ktore beda brane pod uwage
     przez serwis modulowy skanujacy wybrane foldery -> full_file_names; prefix_file_names; suffix_file_names
 * _________________________________________________________________________________________________
 * UWAGA! W przypadku podawania sciezek folderow ktore maja byc skanowane, nalezy uprzednio stworzyc 
 * foldery o podanych sciezkach
 * _________________________________________________________________________________________________
*/
class ModuleManager {

    private:

        /*
         * Tablica do modyfikowania.
         * Przechowuje sciezki do folderow ktore beda skanowane przez modul
         * tj. gdy pojawi sie w folderze plik to zostanie stworzony log, ktory potem zostanie wyslany na serwer 
        */
        const string scan_folder_paths[2]={
            "/home/stud/Documents/lab_files",
            "/home/stud/Documents/lab_files2"
        };

        /*
         * Tablica do modyfikowania.
         * Przechowuje pelne nazwy plikow ktore maja byc uwzgledniane 
         * przez modul skanujacy folder(y)
        */
        const string full_files_names[2]={
            "pkc_zad1",
            "pkc_zad2"
        };

        /*
         * Tablica do modyfikowania.
         * Przechowuje przedrostki nazw plikow, ktore beda tworzone podczas
         * laboratorium i beda brane pod uwage przez modul skanujacy folder(y)
        */
        const string prefix_files_names[2]={
            "pkc_zad",
            "pkc_cw"
        };

        /*
         * Tablica do modyfikowania.
         * Przechowuje przyrostki nazw plikow, ktore beda tworzone podczas
         * laboratorium i beda brane pod uwage przez modul skanujacy folder(y)
        */
        const string suffix_files_names[2]{
            ".py",
            ".txt"
        };

    public:
        ModuleManager();
        ~ModuleManager();
        vector<string> getScanFolderPaths();
        vector<string> getFullFileNames();
        vector<string> getPrefixFileNames();
        vector<string> getSuffixFileNames();
};