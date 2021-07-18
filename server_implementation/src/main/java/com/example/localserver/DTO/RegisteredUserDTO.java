package com.example.localserver.DTO;

public class RegisteredUserDTO {

    private String uniqueCode;
    private String indexNr;

    public RegisteredUserDTO(){}

    public RegisteredUserDTO(String uniqueCode, String indexNum) {
        this.uniqueCode = uniqueCode;
        this.indexNr = indexNum;
    }

    public String getUniqueCode() {
        return uniqueCode;
    }

    public void setUniqueCode(String uniqueCode) {
        this.uniqueCode = uniqueCode;
    }

    public String getIndexNr() {
        return indexNr;
    }

    public void setIndexNr(String indexNr) {
        this.indexNr = indexNr;
    }

    @Override
    public String toString() {
        return "{" + "uniqueCode='" + uniqueCode + '\'' +
                ",indexNum='" + indexNr + '\'' + '}';
    }
}