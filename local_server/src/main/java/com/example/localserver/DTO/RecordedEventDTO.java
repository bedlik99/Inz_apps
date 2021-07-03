package com.example.localserver.DTO;

public class RecordedEventDTO {

    private String indexNum;
    private String registryContent;

    public RecordedEventDTO(String indexNum, String registryContent) {
        this.indexNum = indexNum;
        this.registryContent = registryContent;
    }

    public String getIndexNum() {
        return indexNum;
    }

    public void setIndexNum(String indexNum) {
        this.indexNum = indexNum;
    }

    public String getRegistryContent() {
        return registryContent;
    }

    public void setRegistryContent(String registryContent) {
        this.registryContent = registryContent;
    }
}
