
public class MessageDTO {
    String label;

    public MessageDTO(String label) {
        this.label = label;
    }

    public String getLabel() {
        return label;
    }

    public void setLabel(String label) {
        this.label = label;
    }

    @Override
    public String toString() {
        return "{" + "label='" + label + '\'' + '}';
    }
}
