#include <string>

class IOConfig{
    
private:
    const std::string chars = "\t\n\v\f\r ";
    const std::string logsPath = "/home/cerber/Documents/lab_supervision/identify_lab_data/logs_dir/main_logs";
    const std::string labEndDateFilePath = "/home/cerber/Documents/lab_supervision/identify_lab_data/lab_end_date";

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
    std::string currentDateTime();
    std::string readLabEndDate();
    void getCommandOutput(std::string command,std::vector <std::string> &resultSet);
    
};