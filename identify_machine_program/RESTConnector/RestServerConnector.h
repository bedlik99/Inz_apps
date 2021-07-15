#include <string>
#include <cpprest/http_client.h>
#include "UserDetailsDTO.h"
#include "OPENSSL_AES_256/OpenSSLAesEncryptor.h"

using namespace web;

class RestServerConnector {

private:
    const std::string registerEndpoint = "/registerUser";
    const std::string registrationFilePath = "/home/jan/Documents/inz_dyp/Projekty_C++/working_folder_inz/rgstr_trace/rqrm.txt";
    UserDetailsDTO* initUserDetails = new UserDetailsDTO();
    OpenSSLAesEncryptor* openSSLCryptoUtil = new OpenSSLAesEncryptor();

    json::value returnUserDTOAsJson(std::string userIndex, std::string userCode);
    int executeRequest(json::value json_par, std::string filePath, std::string endpointName);
    std::string encryptAES256WithOpenSSL(std::string strToEncrypt);
    std::string decryptAES256WithOpenSSL(std::string strToDecrypt);
    
public:
    RestServerConnector();
    ~RestServerConnector();
    int sendData(std::string inputIndex, std::string inputCode);

    UserDetailsDTO& getUserDetailsDTO();
    const std::string getRegistrationFilePath();
    void setUserDetailsDTO(UserDetailsDTO userDetailsDTO);

};