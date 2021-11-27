#include "OpenSSLAesEncryptor.h"
#include <openssl/bio.h>
#include <openssl/conf.h>
#include <openssl/err.h>
#include <openssl/evp.h>
#include <algorithm>
#include <stdio.h>
#include <stdlib.h>
#include <time.h>
#include <string>
#include <string.h>
#include <math.h>
#include <iostream>
#include <fstream>
#include <vector>
#include <cassert>
#include <limits>
#include <stdexcept>
#include <cctype>

#if _DEBUG
#pragma comment(lib, "libcrypto64MDd.lib")
#pragma comment(lib, "libssl64MDd.lib")
#else
#pragma comment(lib, "libcrypto64MT.lib")
#pragma comment(lib, "libssl64MT.lib")
#endif

static inline bool is_base64(unsigned char c);

using namespace std;

OpenSSLAesEncryptor::OpenSSLAesEncryptor(){}

static inline bool is_base64(unsigned char c) {
    return (isalnum(c) || (c == '+') || (c == '/'));
}

// base64 encoding
string OpenSSLAesEncryptor::base64_encode(unsigned char *bytes_to_encode, int in_len) {
    string ret;
    int i = 0;
    int j = 0;
    unsigned char char_array_3[3];
    unsigned char char_array_4[4];

    while (in_len--) {
        char_array_3[i++] = *(bytes_to_encode++);
        if (i == 3) {
            char_array_4[0] = (char_array_3[0] & 0xfc) >> 2;
            char_array_4[1] = ((char_array_3[0] & 0x03) << 4) + ((char_array_3[1] & 0xf0) >> 4);
            char_array_4[2] = ((char_array_3[1] & 0x0f) << 2) + ((char_array_3[2] & 0xc0) >> 6);
            char_array_4[3] = char_array_3[2] & 0x3f;

            for (i = 0; (i < 4); i++)
                ret += base64_chars[char_array_4[i]];
            i = 0;
        }
    }

    if (i) {
        for (j = i; j < 3; j++)
            char_array_3[j] = '\0';

        char_array_4[0] = (char_array_3[0] & 0xfc) >> 2;
        char_array_4[1] = ((char_array_3[0] & 0x03) << 4) + ((char_array_3[1] & 0xf0) >> 4);
        char_array_4[2] = ((char_array_3[1] & 0x0f) << 2) + ((char_array_3[2] & 0xc0) >> 6);
        char_array_4[3] = char_array_3[2] & 0x3f;

        for (j = 0; (j < i + 1); j++)
            ret += base64_chars[char_array_4[j]];

        while ((i++ < 3))
            ret += '=';
    }

    return ret;
}

// base64 decoding
string OpenSSLAesEncryptor::base64_decode(string &encoded_string) {
    int in_len = encoded_string.size();
    int i = 0;
    int j = 0;
    int in_ = 0;
    unsigned char char_array_4[4], char_array_3[3];
    string ret;

    while (in_len-- && (encoded_string[in_] != '=') && is_base64(encoded_string[in_])) {
        char_array_4[i++] = encoded_string[in_];
        in_++;
        if (i == 4) {
            for (i = 0; i < 4; i++)
                char_array_4[i] = base64_chars.find(char_array_4[i]);

            char_array_3[0] = (char_array_4[0] << 2) + ((char_array_4[1] & 0x30) >> 4);
            char_array_3[1] = ((char_array_4[1] & 0xf) << 4) + ((char_array_4[2] & 0x3c) >> 2);
            char_array_3[2] = ((char_array_4[2] & 0x3) << 6) + char_array_4[3];

            for (i = 0; (i < 3); i++)
                ret += char_array_3[i];
            i = 0;
        }
    }

    if (i) {
        for (j = i; j < 4; j++)
            char_array_4[j] = 0;

        for (j = 0; j < 4; j++)
            char_array_4[j] = base64_chars.find(char_array_4[j]);

        char_array_3[0] = (char_array_4[0] << 2) + ((char_array_4[1] & 0x30) >> 4);
        char_array_3[1] = ((char_array_4[1] & 0xf) << 4) + ((char_array_4[2] & 0x3c) >> 2);
        char_array_3[2] = ((char_array_4[2] & 0x3) << 6) + char_array_4[3];

        for (j = 0; (j < i - 1); j++) ret += char_array_3[j];
    }


    return ret;
}

int OpenSSLAesEncryptor::encrypt(unsigned char *plaintext, int plaintext_len, unsigned char *key,
                                 unsigned char *iv, unsigned char *ciphertext) {
    EVP_CIPHER_CTX *ctx;

    int len;

    int ciphertext_len;

    /* Create and initialise the context */
    if (!(ctx = EVP_CIPHER_CTX_new()))
        handleErrors();

    /*
     * Initialise the encryption operation. IMPORTANT - ensure you use a key
     * and IV size appropriate for your cipher
     * In this example we are using 256 bit AES (i.e. a 256 bit key). The
     * IV size for *most* modes is the same as the block size. For AES this
     * is 128 bits
     */
    if (1 != EVP_EncryptInit_ex(ctx, EVP_aes_256_cbc(), NULL, key, iv))
        handleErrors();
    
    EVP_CIPHER_CTX_set_padding(ctx, 0);

    /*
     * Provide the message to be encrypted, and obtain the encrypted output.
     * EVP_EncryptUpdate can be called multiple times if necessary
     */
    if (1 != EVP_EncryptUpdate(ctx, ciphertext, &len, plaintext, plaintext_len))
        handleErrors();
    ciphertext_len = len;

    /*
     * Finalise the encryption. Further ciphertext bytes may be written at
     * this stage.
     */
    if (1 != EVP_EncryptFinal_ex(ctx, ciphertext + len, &len))
        handleErrors();
    ciphertext_len += len;

    /* Clean up */
    EVP_CIPHER_CTX_free(ctx);

    return ciphertext_len;
}

void OpenSSLAesEncryptor::handleErrors(void) {
    ERR_print_errors_fp(stderr);
    abort();
}

int OpenSSLAesEncryptor::decrypt(unsigned char *ciphertext, int ciphertext_len, unsigned char *key,
                                 unsigned char *iv, unsigned char *plaintext) {
    EVP_CIPHER_CTX *ctx;

    int len;

    int plaintext_len;

    /* Create and initialise the context */
    if (!(ctx = EVP_CIPHER_CTX_new()))
        handleErrors();

    /*
     * Initialise the decryption operation. IMPORTANT - ensure you use a key
     * and IV size appropriate for your cipher
     * In this example we are using 256 bit AES (i.e. a 256 bit key). The
     * IV size for *most* modes is the same as the block size. For AES this
     * is 128 bits
     */
    if (1 != EVP_DecryptInit_ex(ctx, EVP_aes_256_cbc(), NULL, key, iv))
        handleErrors();

    EVP_CIPHER_CTX_set_padding(ctx, 0);
    /*
     * Provide the message to be decrypted, and obtain the plaintext output.
     * EVP_DecryptUpdate can be called multiple times if necessary.
     */
    if (1 != EVP_DecryptUpdate(ctx, plaintext, &len, ciphertext, ciphertext_len))
        handleErrors();
    plaintext_len = len;

    /*
     * Finalise the decryption. Further plaintext bytes may be written at
     * this stage.
     */
    if (1 != EVP_DecryptFinal_ex(ctx, plaintext + len, &len))
        handleErrors();
    plaintext_len += len;

    /* Clean up */
    EVP_CIPHER_CTX_free(ctx);

    return plaintext_len;
}

bool OpenSSLAesEncryptor::fillStringWithChars(string& str) {
    if (str.length() == 0)
        return false;

    long stringNecessarySize=str.length();

    if(str.length() % 16 != 0)
        stringNecessarySize = findMaxRangeOfStringLength(str.length(), 0, 16);
    
    long numberOfNeededChars = stringNecessarySize - str.length();
    string neededChars = convert_ASCII_To16System(numberOfNeededChars);

    for (long i = 0; i < numberOfNeededChars-1; i++) {
        int randomCharIndex = rand() % 88;
        neededChars = neededChars + writable_chars[randomCharIndex];
    }

    if(numberOfNeededChars!=0)
        str = neededChars + str;

    return true;
}

string OpenSSLAesEncryptor::convert_ASCII_To16System(int charASCIIValue){
    string neededChars="";
    switch (charASCIIValue){
        case 10:
            neededChars = "A";
            break;
        case 11:
            neededChars = "B";
            break;
        case 12:
            neededChars = "C";
            break;
        case 13:
            neededChars = "D";
            break;
        case 14:
            neededChars = "E";
            break;
        case 15:
            neededChars = "F";
            break;
        default:
            neededChars = to_string(charASCIIValue);
            break;
    }
    return neededChars;
}

int OpenSSLAesEncryptor::convert_ASCII_To10System(char charASCIIValue){
    int decimalValue=0;
    switch (charASCIIValue){
        case 'A':
            decimalValue = 10;
            break;
        case 'B':
            decimalValue = 11;
            break;
        case 'C':
            decimalValue = 12;
            break;
        case 'D':
            decimalValue = 13;
            break;
        case 'E':
            decimalValue = 14;
            break;
        case 'F':
            decimalValue = 15;
            break;
        default:
            decimalValue = (int)charASCIIValue - 48;
            break;
    }

    return decimalValue;
}

long OpenSSLAesEncryptor::findMaxRangeOfStringLength(long strLength, long lowRange, long highRange) {
    if (strLength >= lowRange && strLength < highRange) {
        return highRange;
    }
    lowRange += 16;
    highRange += 16;
    return findMaxRangeOfStringLength(strLength, lowRange, highRange);
}

void OpenSSLAesEncryptor::removeCharsFromString(string& str) {
    if(str.at(0) != '{' && str.at(0) != '['){
        char firstDigitASCII = str.at(0);
        int startingNumberOfChars = convert_ASCII_To10System(firstDigitASCII);
        str = str.substr(startingNumberOfChars);
    }
}

string OpenSSLAesEncryptor::encryptAES256WithOpenSSL(string strToEncrypt){
    /* A 256 bit key */
    /* A 128 bit IV */
    int key_arr[] = {219,113,24,135,39,29,126,237,62,41,40,80,13,196,236,161,67,200,70,232,196,148,143,13,128,99,251,43,176,0,198,38};
    int iv_arr[] = {91,158,142,6,25,84,195,85,107,12,209,105,219,211,177,89};
    string created_key="",created_iv="";
    for(int i=0;i<sizeof(key_arr)/sizeof(key_arr[0]);i++){
        created_key=created_key+(char)key_arr[i];
    }
    for(int i=0;i<sizeof(iv_arr)/sizeof(iv_arr[0]);i++){
        created_iv=created_iv+(char)iv_arr[i];
    }
    string encoded64String="";
    if(fillStringWithChars(strToEncrypt)){
        unsigned char* charsToEncrypt = (unsigned char *)strToEncrypt.c_str();
        /*
        * Buffer for ciphertext. Ensure the buffer is long enough for the
        * ciphertext which may be longer than the plaintext, depending on the
        * algorithm and mode.
        */
        unsigned char ciphertext[bufferLength];
        /* Encrypt the plaintext */
        int ciphertext_l = encrypt(
        charsToEncrypt, 
        strlen((char *)charsToEncrypt),
        (unsigned char *)created_key.c_str(),
        (unsigned char *)created_iv.c_str(),
        ciphertext);

        encoded64String = base64_encode(ciphertext,ciphertext_l);
    }
    return encoded64String;
}

string OpenSSLAesEncryptor::decryptAES256WithOpenSSL(string encoded64StrToDecrypt){
    /* A 256 bit key */
    /* A 128 bit IV */
    string key,iv,decryptedStringToReturn;
    readSecrects(key,iv);

    /* Buffer for the decrypted text */
    unsigned char decryptedtext[bufferLength];
        
    /*Encrypted ciphertext to be decrypted*/
    unsigned char encryptedtextTmp[bufferLength];

    string decoded64String = base64_decode(encoded64StrToDecrypt);

    /* Decrypt the ciphertext */
    int decryptedtext_l = decrypt(
    (unsigned char*)decoded64String.c_str(),
    decoded64String.length(),
    (unsigned char *)key.c_str(),
    (unsigned char *)iv.c_str(),
    decryptedtext);

    /* Add a NULL terminator. We are expecting printable text */
    decryptedtext[decryptedtext_l] = '\0';
    decryptedStringToReturn = (char *)decryptedtext;

    removeCharsFromString(decryptedStringToReturn);

    return decryptedStringToReturn;
}

void OpenSSLAesEncryptor::readSecrects(string& key, string& iv){
    string tmpStr;
    int it=0;
    ifstream secretFile(secretPath);
    while (getline(secretFile, tmpStr)) {
        switch (it) {
            case 0:
                key = tmpStr;
                break;
            case 1:
                iv = tmpStr;
                break;        
            default:
                break;
        }
        it++;
        if(it==2) break;
    }   
    secretFile.close();
}