#include <string>

class UserDetailsDTO {

private:
    std::string indexNr;
    std::string uniqueCode;

public:
    UserDetailsDTO();
    UserDetailsDTO(std::string index,std::string uniqueCode);

    const std::string& getIndex();
    void setIndex(std::string indexNr);

    const std::string& getUniqueCode();
    void setUniqueCode(std::string uniqueCode);

    const std::string toJson();
};