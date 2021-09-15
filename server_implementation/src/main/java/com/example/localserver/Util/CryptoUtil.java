package com.example.localserver.Util;

import javax.crypto.BadPaddingException;
import javax.crypto.Cipher;
import javax.crypto.IllegalBlockSizeException;
import javax.crypto.NoSuchPaddingException;
import javax.crypto.spec.IvParameterSpec;
import javax.crypto.spec.SecretKeySpec;
import java.security.InvalidAlgorithmParameterException;
import java.security.InvalidKeyException;
import java.security.NoSuchAlgorithmException;
import java.util.Base64;

public abstract class CryptoUtil {

    private static final byte[] byteArrayKey = {109, 90, 113, 52, 116, 55, 119, 33, 122, 37, 67, 42, 70, 45, 74, 64, 78, 99, 82, 102, 85, 106, 88, 110, 50, 114, 53, 117, 56, 120, 47, 65};
    private static final IvParameterSpec iv = new IvParameterSpec(new byte[]{'4', 'q', 'U', 'T', 'M', 'L', 'k', 'E', '1', '5', 'P', 'X', 'g', '6', 'B', 'm'});
    private static final SecretKeySpec key = new SecretKeySpec(byteArrayKey, "AES");
    private static final String algorithm = "AES/CBC/NoPadding";
    private static final String writable_chars = "P_3Auw|g!4EHS1.#W0<+oR?OIZ'9k*6=hCUGtTbQN(f;7/%lr8>LzD2$sy5p@Mq,acBdveKV)nm~ij`Y:&JXF^x-";

    public static String encrypt(String input)
            throws NoSuchPaddingException, NoSuchAlgorithmException,
            InvalidAlgorithmParameterException, InvalidKeyException,
            BadPaddingException, IllegalBlockSizeException {

        Cipher cipher = Cipher.getInstance(algorithm);
        cipher.init(Cipher.ENCRYPT_MODE, key, iv);
        byte[] cipherText = cipher.doFinal(input.getBytes());
        return Base64.getEncoder()
                .encodeToString(cipherText);
    }

    public static String decrypt(String cipherText)
            throws NoSuchPaddingException, NoSuchAlgorithmException,
            InvalidAlgorithmParameterException, InvalidKeyException,
            BadPaddingException, IllegalBlockSizeException {

        Cipher cipher = Cipher.getInstance(algorithm);
        cipher.init(Cipher.DECRYPT_MODE, key, iv);
        byte[] plainText = cipher.doFinal(Base64.getDecoder()
                .decode(cipherText));
        return new String(plainText);
    }

    public static String getWritable_chars() {
        return writable_chars;
    }

    public static String convert_ASCII_To16System(int charASCIIValue){
        String neededChars="";
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
                neededChars = String.valueOf(charASCIIValue);
                break;
        }
        return neededChars;
    }

    public static int convert_ASCII_To10System(String charASCIIValue){
        int decimalValue=0;
        switch (charASCIIValue){
            case "A":
                decimalValue = 10;
                break;
            case "B":
                decimalValue = 11;
                break;
            case "C":
                decimalValue = 12;
                break;
            case "D":
                decimalValue = 13;
                break;
            case "E":
                decimalValue = 14;
                break;
            case "F":
                decimalValue = 15;
                break;
            default:
                decimalValue = Integer.parseInt(charASCIIValue);
                break;
        }
        return decimalValue;
    }
    /*
       public static SecretKey generateKey(int n) throws NoSuchAlgorithmException {
        KeyGenerator keyGenerator = KeyGenerator.getInstance("AES");
        keyGenerator.init(n);
        SecretKey key = keyGenerator.generateKey();
        return key;
    }

    public static SecretKey getKeyFromPassword(String password, String salt)
            throws NoSuchAlgorithmException, InvalidKeySpecException {

        SecretKeyFactory factory = SecretKeyFactory.getInstance("PBKDF2WithHmacSHA256");
        KeySpec spec = new PBEKeySpec(password.toCharArray(), salt.getBytes(), 65536, 256);
        SecretKey secret = new SecretKeySpec(factory.generateSecret(spec)
                .getEncoded(), "AES");
        return secret;
    }

    public static IvParameterSpec generateIv() {
        byte[] iv = new byte[16];
        new SecureRandom().nextBytes(iv);
        return new IvParameterSpec(new byte[16]);
    }
*/
}