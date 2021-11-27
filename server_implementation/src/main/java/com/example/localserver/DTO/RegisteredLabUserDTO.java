package com.example.localserver.DTO;

public class RegisteredLabUserDTO {

    private String uniqueCode;
    private String email;

    public RegisteredLabUserDTO(){}

    public RegisteredLabUserDTO(String uniqueCode, String email) {
        this.uniqueCode = uniqueCode;
        this.email = email;
    }

    public String getUniqueCode() {
        return uniqueCode;
    }

    public void setUniqueCode(String uniqueCode) {
        this.uniqueCode = uniqueCode;
    }

    public String getEmail() {
        return email;
    }

    public void setEmail(String mail) {
        this.email = mail;
    }

    @Override
    public String toString() {
        return "{" + "uniqueCode='" + uniqueCode + '\'' +
                ",email='" + email + '\'' + '}';
    }
}