package com.example.localserver.DTO;

public class RegisteredUserDTO {

    private String uniqueCode;
    private String indexNum;

    public RegisteredUserDTO(){}

    public RegisteredUserDTO(String uniqueCode, String indexNum) {
        this.uniqueCode = uniqueCode;
        this.indexNum = indexNum;
    }

    public String getUniqueCode() {
        return uniqueCode;
    }

    public void setUniqueCode(String uniqueCode) {
        this.uniqueCode = uniqueCode;
    }

    public String getIndexNum() {
        return indexNum;
    }

    public void setIndexNum(String indexNum) {
        this.indexNum = indexNum;
    }
}