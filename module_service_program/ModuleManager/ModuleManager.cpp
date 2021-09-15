#include <string>
#include <vector>
#include "ModuleManager.h"

using namespace std;

ModuleManager::ModuleManager(){}
ModuleManager::~ModuleManager(){}

vector<string> ModuleManager::getScanFolderPaths(){
    int arraySize = sizeof(scan_folder_paths)/sizeof(scan_folder_paths[0]);
    vector<string> vectorPaths(scan_folder_paths,scan_folder_paths+arraySize);
    return vectorPaths;
}

vector<string> ModuleManager::getFullFileNames(){
    int arraySize = sizeof(full_files_names)/sizeof(full_files_names[0]);
    vector<string> vectorFullFileNames(full_files_names,full_files_names+arraySize);
    return vectorFullFileNames;
}

vector<string> ModuleManager::getPrefixFileNames(){
    int arraySize = sizeof(prefix_files_names)/sizeof(prefix_files_names[0]);
    vector<string> vectorPrefixFileNames(prefix_files_names,prefix_files_names+arraySize);
    return vectorPrefixFileNames;
}

vector<string> ModuleManager::getSuffixFileNames(){
    int arraySize = sizeof(suffix_files_names)/sizeof(suffix_files_names[0]);
    vector<string> vectorSuffixFileNames(suffix_files_names,suffix_files_names+arraySize);
    return vectorSuffixFileNames;
}