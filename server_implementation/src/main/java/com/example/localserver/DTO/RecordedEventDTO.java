package com.example.localserver.DTO;

public class RecordedEventDTO {

    private String indexNr;
    private String registryContent;

    public RecordedEventDTO(){}

    public RecordedEventDTO(String indexNr, String registryContent) {
        this.indexNr = indexNr;
        this.registryContent = registryContent;
    }

    public String getIndexNr() {
        return indexNr;
    }

    public void setIndexNr(String indexNr) {
        this.indexNr = indexNr;
    }

    public String getRegistryContent() {
        return registryContent;
    }

    public void setRegistryContent(String registryContent) {
        this.registryContent = registryContent;
    }

    @Override
    public String toString() {
        return "{" + "indexNr='" + indexNr + '\'' +
                ", registryContent='" + registryContent + '\'' +
                '}';
    }
}
