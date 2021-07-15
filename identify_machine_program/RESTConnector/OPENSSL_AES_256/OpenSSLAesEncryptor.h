#include <string>

class OpenSSLAesEncryptor {
private:
    const std::string base64_chars = 
    "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
    "abcdefghijklmnopqrstuvwxyz"
    "0123456789+/";
    const static int bufferLength = 1024;

    /* A 256 bit key */
    /*DONT HARDCODE IN REAL APP!!!*/
    unsigned char* key;

    /* A 128 bit IV */
    /*DONT HARDCODE IN REAL APP!!!*/
    unsigned char* iv;

    unsigned char ciphertextStr[bufferLength];
    int ciphertext_len;

    unsigned char decryptedtextStr[bufferLength];
    int decryptedtext_len;

public:
    OpenSSLAesEncryptor();
    std::string base64_encode(unsigned char* bytes_to_encode, int in_len);
    std::string base64_decode(std::string & encoded_string);
    void handleErrors(void);
    int decrypt(unsigned char *ciphertext, int ciphertext_len, unsigned char *key,unsigned char *iv, unsigned char *plaintext);
    int encrypt(unsigned char *plaintext, int plaintext_len, unsigned char *key, unsigned char *iv, unsigned char *ciphertext);

    unsigned char* getKey();
    unsigned char* getIV();
    std::string getCipherTextAsString();
    int& getCipherTextLength(); 
    std::string getDecryptedTextAsString();
    int& getDecryptedTextLength();
    const int getBufferLength();  

    void setKey(unsigned char* keyToBeSetted);
    void setIV(unsigned char* ivToBeSetted);
    void setCipherText(unsigned char cipherTextToSet[bufferLength],int length);
    void setCipherTextLength(int cipherLength); 
    void setDecryptedText(unsigned char decryptedTextToSet[bufferLength],int length);
    void setDecryptedTextLength(int decryptedLength);


};