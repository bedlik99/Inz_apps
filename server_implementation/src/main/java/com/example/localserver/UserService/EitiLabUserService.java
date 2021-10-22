package com.example.localserver.UserService;

import com.example.localserver.DTO.MessageDTO;
import com.example.localserver.DTO.RecordedEventDTO;
import com.example.localserver.DTO.RegisteredLabUserDTO;
import com.example.localserver.Entity.RecordedEvent;
import com.example.localserver.Entity.RegisteredUser;
import com.example.localserver.Repositories.EitiLabUserRepository;
import com.example.localserver.Repositories.RecordedEventRepository;
import com.example.localserver.Util.CryptoUtil;
import com.google.gson.Gson;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;

import javax.crypto.BadPaddingException;
import javax.crypto.IllegalBlockSizeException;
import javax.crypto.NoSuchPaddingException;
import java.security.InvalidAlgorithmParameterException;
import java.security.InvalidKeyException;
import java.security.NoSuchAlgorithmException;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.Comparator;
import java.util.List;
import java.util.Set;

@Service
public class EitiLabUserService {

    private final EitiLabUserRepository eitiLabUserRepository;
    private final RecordedEventRepository recordedEventRepository;

    @Autowired
    public EitiLabUserService(EitiLabUserRepository eitiLabUserRepository, RecordedEventRepository recordedEventRepository) {
        this.eitiLabUserRepository = eitiLabUserRepository;
        this.recordedEventRepository = recordedEventRepository;
    }

    public List<RecordedEventDTO> findStudentRegistries(String email){
        List<RecordedEventDTO> recordedLogsList = new ArrayList<>();
        RegisteredUser searchedUser = eitiLabUserRepository.findStudentByEmail(email);
        if(email != null && searchedUser != null){
            if(email.trim().length() == 18){
                Set<RecordedEvent> recordedEvents = searchedUser.getEventRegistries();
                recordedEvents.stream()
                        .sorted(Comparator.comparing(RecordedEvent::getDateTime))
                        .forEach(el -> recordedLogsList.add(new RecordedEventDTO(email,el.getRegistryContent())));
            }
        }
        return recordedLogsList;
    }

    public ResponseEntity<String> processUserInitData(MessageDTO encryptedMessage) {
        RegisteredLabUserDTO registeredUser =
                (RegisteredLabUserDTO) decryptMessage(encryptedMessage.getValue(),true);
        if (registeredUser != null) {
            if (validateUserData(registeredUser)) {
                RegisteredUser userToSave = new RegisteredUser(registeredUser.getMail(), registeredUser.getUniqueCode());
                eitiLabUserRepository.save(userToSave);
                recordedEventRepository.save(new RecordedEvent("Maszyna zostala zarejestrowana", LocalDateTime.now(), userToSave));
                return ResponseEntity.status(HttpStatus.OK).body("");
            }
            return ResponseEntity.status(HttpStatus.NOT_ACCEPTABLE).body("");
        }
        return ResponseEntity.status(HttpStatus.NOT_ACCEPTABLE).body("");
    }

    public void processEventContent(MessageDTO encryptedMessage) {
        RecordedEventDTO recordedEvent =
                (RecordedEventDTO) decryptMessage(encryptedMessage.getValue(),false);
        RegisteredUser searchedUser=null;
        if (recordedEvent != null && !recordedEvent.getMail().isEmpty()) {
            searchedUser = eitiLabUserRepository.findStudentByEmail(recordedEvent.getMail());
            if(searchedUser != null)
                recordedEventRepository.save(new RecordedEvent(
                        recordedEvent.getRegistryContent(), LocalDateTime.now(), searchedUser));
        }
    }

    private String encryptMessage(String str) {
        String encryptedResponse = "";
        str = fillStringWithChars(str);
        try {
            encryptedResponse = CryptoUtil.encrypt(str);
        } catch (InvalidAlgorithmParameterException | NoSuchPaddingException | IllegalBlockSizeException
                | NoSuchAlgorithmException | BadPaddingException | InvalidKeyException e) {
            e.printStackTrace();
        }
        return encryptedResponse;
    }

    private Object decryptMessage(String encryptedMessage,boolean isRegistration) {
        try {
            String decryptedMessage = CryptoUtil.decrypt(encryptedMessage);
            decryptedMessage = removeCharsFromString(decryptedMessage);
            if(isRegistration)
                return new Gson().fromJson(decryptedMessage, RegisteredLabUserDTO.class);
            else
                return new Gson().fromJson(decryptedMessage, RecordedEventDTO.class);
        } catch (InvalidAlgorithmParameterException | NoSuchPaddingException | IllegalBlockSizeException
                | NoSuchAlgorithmException | BadPaddingException | InvalidKeyException e) {
            e.printStackTrace();
        }
        return null;
    }

    private boolean validateUserData(RegisteredLabUserDTO registeredLabUserDTO) {
        return registeredLabUserDTO.getMail().matches("[0-9]{8}@pw.edu.pl") &&
                registeredLabUserDTO.getUniqueCode().length() == 8;
    }

    private String fillStringWithChars(String str) {
        if (str.length() == 0) {
            return "";
        }
        long stringNecessarySize = str.length();
        stringNecessarySize = (stringNecessarySize % 16 != 0) ? findMaxRangeOfStringLength(str.length(), 0, 16) : stringNecessarySize;
        long numberOfNeededChars = stringNecessarySize - str.length();
        StringBuilder neededChars = new StringBuilder(CryptoUtil.convert_ASCII_To16System((int)numberOfNeededChars));
        for (long i = 0; i < numberOfNeededChars-1; i++) {
            int randomCharIndex = (int)Math.floor(Math.random()*88);
            neededChars.append(CryptoUtil.getWritable_chars().charAt(randomCharIndex));
        }
        str = numberOfNeededChars!=0 ? (neededChars + str) : str;
        return str;
    }

    private long findMaxRangeOfStringLength(long strLength, long lowRange, long highRange) {
        if (strLength >= lowRange && strLength < highRange) {
            return highRange;
        }
        lowRange += 16;
        highRange += 16;
        return findMaxRangeOfStringLength(strLength, lowRange, highRange);
    }

    private String removeCharsFromString(String str) {
        if(str.charAt(0) != '{' && str.charAt(0) != '['){
            String firstASCIIChar = str.substring(0, 1);
            int decimalValue = CryptoUtil.convert_ASCII_To10System(firstASCIIChar);
            str = str.substring(decimalValue);
        }
        return str;
    }
}