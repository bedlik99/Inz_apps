package com.example.localserver.DTO;

public class RegisteredLabUserDTO {

    private String uniqueCode;
    private String mail;

    public RegisteredLabUserDTO(){}

    public RegisteredLabUserDTO(String uniqueCode, String indexNum) {
        this.uniqueCode = uniqueCode;
        this.mail = indexNum;
    }

    public String getUniqueCode() {
        return uniqueCode;
    }

    public void setUniqueCode(String uniqueCode) {
        this.uniqueCode = uniqueCode;
    }

    public String getMail() {
        return mail;
    }

    public void setMail(String mail) {
        this.mail = mail;
    }

    @Override
    public String toString() {
        return "{" + "uniqueCode='" + uniqueCode + '\'' +
                ",mail='" + mail + '\'' + '}';
    }
}