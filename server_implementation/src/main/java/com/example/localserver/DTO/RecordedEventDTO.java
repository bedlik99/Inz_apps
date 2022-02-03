package com.example.localserver.DTO;

public class RecordedEventDTO {

    private String uniqueCode;
    private String registryContent;

    public RecordedEventDTO(){}

    public RecordedEventDTO(String uniqueCode, String registryContent) {
        this.uniqueCode = uniqueCode;
        this.registryContent = registryContent;
    }

    public String getUniqueCode() {
        return uniqueCode;
    }

    public void setUniqueCode(String uniqueCode) {
        this.uniqueCode = uniqueCode;
    }

    public String getRegistryContent() {
        return registryContent;
    }

    public void setRegistryContent(String registryContent) {
        this.registryContent = registryContent;
    }

    @Override
    public String toString() {
        return "{" + "uniqueCode='" + uniqueCode + '\'' +
                ", registryContent='" + registryContent + '\'' +
                '}';
    }
}
