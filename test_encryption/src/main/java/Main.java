import javax.crypto.spec.SecretKeySpec;
import java.nio.charset.StandardCharsets;
import java.security.GeneralSecurityException;

public class Main {

    public static void main(String[] args) {

        String id = "300480";
        byte[] byteArrayWordToEncrypt = id.getBytes(StandardCharsets.UTF_8);
        String secretKeyString = "mZq4t7w!z%C*F-J@NcRfUjXn2r5u8x/A";
        byte[] byteArrayKey = {109,90,113,52,116,55,119,33,122,37,67,42,70,45,74,64,
                78,99,82,102,85,106,88,110,50,114,53,117,56,120,47,65};
        SecretKeySpec key = new SecretKeySpec(byteArrayKey, "AES");

        try {
            String inputString = "B0123456789{\"mail\":\"01143823@pw.edu.pl\",\"uniqueCode\":\"abcdefgh\"}";
            System.out.println("\nInput String: " + inputString);
            String encryptedString = CryptoUtil.encrypt("AES/CBC/NoPadding",inputString,key);
            System.out.println("Encrypted Input String: " + encryptedString);
            String decryptedString = CryptoUtil.decrypt("AES/CBC/NoPadding",encryptedString,key);
            System.out.println("Decrypted Input String: " + decryptedString);
        }catch (GeneralSecurityException e){
            e.printStackTrace();
        }
    }
}