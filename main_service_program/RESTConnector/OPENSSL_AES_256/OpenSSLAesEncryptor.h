#include <string>

class OpenSSLAesEncryptor {
private:
    const std::string base64_chars = 
    "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
    "abcdefghijklmnopqrstuvwxyz"
    "0123456789+/";
    const static int bufferLength = 1024;
    const std::string secretPath = "/home/jan/Documents/inz_dyp/Projekty_C++/working_folder_inz/scrtk/keys";

    std::string base64_encode(unsigned char* bytes_to_encode, int in_len);
    std::string base64_decode(std::string & encoded_string);
    void handleErrors(void);
    int decrypt(unsigned char *ciphertext, int ciphertext_len, unsigned char *key,unsigned char *iv, unsigned char *plaintext);
    int encrypt(unsigned char *plaintext, int plaintext_len, unsigned char *key, unsigned char *iv, unsigned char *ciphertext);
    void readSecrects(std::string& key, std::string& iv);
public:
    OpenSSLAesEncryptor();
    std::string encryptAES256WithOpenSSL(std::string strToEncrypt);
    std::string decryptAES256WithOpenSSL(std::string encoded64StrToDecrypt);
    const std::string getSecretPath();

};