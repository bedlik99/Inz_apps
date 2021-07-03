#include <string>

class IOConfig{
private:
std::string chars = "\t\n\v\f\r ";
std::string& ltrim(std::string& str);
std::string& rtrim(std::string& str);

public:
IOConfig();
bool doesFileExist(const std::string& name);
bool isSecretFileEmpty(const std::string secretFile, bool &wasSecretFileInitProperly);
std::string& trim(std::string& str);
 
};