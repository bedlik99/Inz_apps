#include <string>
#include <cpprest/http_client.h>
#include "UserDetailsDTO.h"

using namespace web;  

class RestServerConnector {

private:
    const std::string registerEndpoint = "/registerUser";
    const std::string registrationFilePath = "/home/jan/Documents/inz_dyp/Projekty_C++/working_folder_inz/rgstr_trace/rqrm.txt";
    UserDetailsDTO initUserDetails = UserDetailsDTO();

    json::value returnUserDTOAsJson(std::string userIndex, std::string userCode);
    int executeRequest(json::value json_par, std::string filePath, std::string endpointName);
public:
    RestServerConnector();
    //tutaj chyba destruktora trzeba napisac zeby wyczyscil obiekt UserDetailsDTO
    int sendData(std::string inputIndex, std::string inputCode);

    UserDetailsDTO& getUserDetailsDTO();
    const std::string getRegistrationFilePath();
    void setUserDetailsDTO(UserDetailsDTO userDetailsDTO);

};