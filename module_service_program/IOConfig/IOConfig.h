#include <string>

class IOConfig{
    
private:
    const std::string chars = "\t\n\v\f\r ";
    
    std::string ltrim(std::string str);
    std::string rtrim(std::string str);

public:
    IOConfig();
    bool doesFileExist(const std::string& name);
    bool isFileEmpty(const std::string filePath);
    std::string trim(std::string str);
    std::string get_file_size(const char *filename);
    int getProcIdByName(std::string procName);
    bool areCredentialsPresent();

};