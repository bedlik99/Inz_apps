#include <SFML/Graphics.hpp>
#include <SFML/Window.hpp>
#include <SFML/System.hpp>
#include <sys/ptrace.h>
#include <boost/algorithm/string.hpp>
#include <iostream>
#include <unistd.h>
#include <fstream>
#include <string>
#include <cmath>
#include <dirent.h>
#include <time.h>
#include <sys/signal.h>
#include "GraphicManagement/GraphicManager.h"
#include "IOConfig/IOConfig.h"

void continueExecutionV();
int  continueExecutionI();
void endExecution(int status);
void refreshDrawedObjects();
void createRegisterLog(std::string emailInput, std::string codeInput);
void handleInfoWindow(std::string errorInfo,std::string windowTitle);
bool is500Error();
int handleApprovalWindow();
int init();
void cleanup();

const static std::string thisProcessName = "IdentifyOnStart";
const static std::string logsFilePath = "/home/cerber/Documents/lab_supervision/identify_lab_data/logs_dir/main_logs";
static sf::RenderWindow mainWindow(sf::VideoMode(540, 650), "Registration",sf::Style::Titlebar);
static sf::String emailInput;
static sf::String codeInput;  
static sf::String tmpEmailCursor="_";
static sf::String tmpCodeCursor="_";  
static bool isEmail = true;
static bool secondHasElapsed = true;
static GraphicManager* graphicManager = nullptr;
static IOConfig ioConfig;

void continueExecutionV(){}
int  continueExecutionI(){return 0;}
void endExecution(int status){
    cleanup();
    exit(status);
}

int main() {
    init() != 0 ? endExecution(-1) : continueExecutionV();
    int ASCII_DEC_CODE = 0; 
    int restCommunicationReturnCode = 0;
    while (mainWindow.isOpen()) {
        
        sf::Event event;
        refreshDrawedObjects();
        while (mainWindow.waitEvent(event) ) {
    
            switch (event.type){
                case sf::Event::TextEntered: 
                    ASCII_DEC_CODE = static_cast<int>(event.key.code);
 
                        if(ASCII_DEC_CODE == 13){
                            if(!ioConfig.areCredentialsPresent()){
                                if(emailInput.getSize()==8 && codeInput.getSize()==8){
                                    if(handleApprovalWindow()==1){
                                        createRegisterLog(emailInput,codeInput);
                                        kill(getpid(),SIGSTOP);
                                        if(!ioConfig.areCredentialsPresent()){
                                            handleInfoWindow("No user found. Check your credentials and try again.", "Authentication error");
                                        }else if(is500Error()){
                                            handleInfoWindow("Server Error. For unknown reason server couldn't respond to your request.", "Internal server error");
                                            std::ofstream outputLogsFile;
                                            outputLogsFile.open(logsFilePath,std::ios::trunc);
                                            outputLogsFile.close();
                                        }else{
                                            handleInfoWindow("Registration was successful.", "Success");
                                        }
                                    }
                                }
                            }else{
                                endExecution(0);
                            }

                        }

                        if(ASCII_DEC_CODE == 9){
                            isEmail = !isEmail;
                            if(isEmail){
                                graphicManager->getArrowLinePointer().setPosition(300,41);
                            }else{
                                graphicManager->getArrowLinePointer().setPosition(350,91);  
                            }
                            refreshDrawedObjects();
                        }

                        if(isEmail && ((ASCII_DEC_CODE >=48 && ASCII_DEC_CODE <=57) || ASCII_DEC_CODE==8)) {
                            if(ASCII_DEC_CODE != 8 && emailInput.getSize()<8) {
                                emailInput += event.text.unicode;
                                tmpEmailCursor.insert(0," ");
                                graphicManager->getCursorEmailPointer().setString(tmpEmailCursor);
                                graphicManager->getEmailInputText().setString(emailInput);

                            }else if(emailInput.getSize() > 0 && ASCII_DEC_CODE == 8){
                                emailInput = emailInput.substring(0,emailInput.getSize()-1);
                                tmpEmailCursor = tmpEmailCursor.substring(1);
                                graphicManager->getCursorEmailPointer().setString(tmpEmailCursor);
                                graphicManager->getEmailInputText().setString(emailInput);
                            }
                            if(emailInput.getSize()==8){
                                graphicManager->getEmailApprovalLabel().setFillColor(sf::Color(0,255,8));
                            }else{
                                graphicManager->getEmailApprovalLabel().setFillColor(sf::Color(255, 5, 0));
                            }

                        }else if(!isEmail && ((ASCII_DEC_CODE >=33 && ASCII_DEC_CODE <=126) || ASCII_DEC_CODE==8)){
                            if(ASCII_DEC_CODE != 8 && codeInput.getSize()<8){
                                codeInput += event.text.unicode;
                                tmpCodeCursor.insert(0," ");
                                graphicManager->getCursorCodePointer().setString(tmpCodeCursor);
                                graphicManager->getCodeInputText().setString(codeInput);

                            }else if (codeInput.getSize() > 0 && ASCII_DEC_CODE == 8){
                            codeInput = codeInput.substring(0, codeInput.getSize()-1);
                                tmpCodeCursor = tmpCodeCursor.substring(1);
                                graphicManager->getCursorCodePointer().setString(tmpCodeCursor);
                                graphicManager->getCodeInputText().setString(codeInput);   
                            }
                            if(codeInput.getSize()==8){
                                graphicManager->getCodeApprovalLabel().setFillColor(sf::Color(0,255,8));
                            }else{
                                graphicManager->getCodeApprovalLabel().setFillColor(sf::Color(255, 5, 0));
                            }
                        }
                    refreshDrawedObjects();
                    break;  
            }   
            refreshDrawedObjects();
        }
    }
    cleanup();
    return 0;
}

void createRegisterLog(std::string emailInput, std::string codeInput){
    if(ioConfig.trim(emailInput).length()==8 && ioConfig.trim(codeInput).length()==8){
        std::ofstream outputLogsFile;
        outputLogsFile.open(logsFilePath,std::ios::app);
        emailInput = emailInput+"@pw.edu.pl";
        std::string registryInfo = emailInput + " " + codeInput;
        outputLogsFile << registryInfo << std::endl;
        outputLogsFile.close();
    }
}

bool is500Error(){
    std::string credentials("");
    std::ifstream logsFile(logsFilePath);
    std::getline(logsFile,credentials);
    logsFile.close();
    if(ioConfig.trim(credentials).compare("500")==0)
        return true;
    return false;
}

void handleInfoWindow(std::string info, std::string windowTitle){
    sf::RenderWindow infoWindow(sf::VideoMode(650, 500), windowTitle,sf::Style::Titlebar);
    graphicManager->getErrorText().setString(info);
    infoWindow.setPosition(sf::Vector2i(2, 2));
    infoWindow.setKeyRepeatEnabled(false);
    infoWindow.setFramerateLimit(60);
    mainWindow.setVisible(false);

    while (infoWindow.isOpen()) {
        sf::Event event;
        infoWindow.clear(sf::Color(230, 230, 230));
        infoWindow.draw(graphicManager->getErrorText());
        infoWindow.draw(graphicManager->getErrorGoBackText());
        infoWindow.display();
        while (infoWindow.waitEvent(event)) {
            switch (event.type){
                case sf::Event::TextEntered:
                    if(static_cast<int>(event.key.code) == 13){
                        infoWindow.close();
                    }
                    break;
            }
            infoWindow.clear(sf::Color(230, 230, 230));
            infoWindow.draw(graphicManager->getErrorText());
            infoWindow.draw(graphicManager->getErrorGoBackText());
            infoWindow.display();         
        }
    }
    mainWindow.setPosition(sf::Vector2i(2, 2));
    mainWindow.setVisible(true);
}

int handleApprovalWindow(){
    sf::RenderWindow confirmationWindow(sf::VideoMode(400, 500), "Confirm Action",sf::Style::Titlebar);
    int ASCII_DEC_CODE;
    int return_code=0;
    graphicManager->getConfirmInputText().setString("");
    confirmationWindow.setPosition(sf::Vector2i(2, 2));
    confirmationWindow.setKeyRepeatEnabled(false);
    confirmationWindow.setFramerateLimit(60);
    mainWindow.setVisible(false);

    while (confirmationWindow.isOpen()) {
        sf::Event event;
        confirmationWindow.clear(sf::Color(230, 230, 230));
        confirmationWindow.draw(graphicManager->getConfirmationText());
        confirmationWindow.draw(graphicManager->getConfirmInputText());
        confirmationWindow.draw(graphicManager->getConfirmEnterText());
        confirmationWindow.display();
        while (confirmationWindow.waitEvent(event)) {
                switch (event.type){
                    case sf::Event::TextEntered:
                        ASCII_DEC_CODE = static_cast<int>(event.key.code);

                        if(ASCII_DEC_CODE == 121 || ASCII_DEC_CODE == 110 || ASCII_DEC_CODE ==78 || ASCII_DEC_CODE == 89){  
                            return_code = (ASCII_DEC_CODE == 121 || ASCII_DEC_CODE == 89) ? 1 : 0;
                            graphicManager->getConfirmInputText().setString(event.text.unicode);
                            confirmationWindow.clear(sf::Color(230, 230, 230));
                            confirmationWindow.draw(graphicManager->getConfirmationText());
                            confirmationWindow.draw(graphicManager->getConfirmInputText());
                            confirmationWindow.draw(graphicManager->getConfirmEnterText());
                            confirmationWindow.display();
                        }
                        if(ASCII_DEC_CODE == 13 && !graphicManager->getConfirmInputText().getString().isEmpty()){
                            confirmationWindow.close();
                        }
                        break;
                }
            confirmationWindow.clear(sf::Color(230, 230, 230));
            confirmationWindow.draw(graphicManager->getConfirmationText());
            confirmationWindow.draw(graphicManager->getConfirmInputText());
            confirmationWindow.draw(graphicManager->getConfirmEnterText());
            confirmationWindow.display();
        }
    }
    mainWindow.setPosition(sf::Vector2i(2, 2));
    if(return_code==0)
        mainWindow.setVisible(true);

    return return_code;
}

bool isRegistrationAlreadyRunning(){
    std::vector <std::string> resultSet;
    ioConfig.getCommandOutput("pgrep IdentifyOnStart",resultSet); 
    for(std::string &value: resultSet) {
        int receivedPid = std::stoi(value);
        int thisProgramPid = (int)getpid();
        if(receivedPid != thisProgramPid)
            return true;
        return false;
    }
}

int init(){
    mainWindow.setVisible(false);
    ioConfig.currentDateTime().compare(ioConfig.readLabEndDate()) >= 0 ? endExecution(0) : continueExecutionV();
    (ioConfig.isFileEmpty(logsFilePath) && !ioConfig.areCredentialsPresent()) ? continueExecutionV() : endExecution(0);
    isRegistrationAlreadyRunning() ? endExecution(0) : continueExecutionV();
    // sleep(10);
    (ioConfig.getProcIdByName("MainService") == -1 || ioConfig.getProcIdByName("ModuleService") == -1) ? kill(getpid(),SIGSTOP) : continueExecutionI();

    mainWindow.setPosition(sf::Vector2i(2, 2));
    mainWindow.setVisible(true);
    mainWindow.setKeyRepeatEnabled(false);
    mainWindow.setFramerateLimit(60);

    graphicManager = new GraphicManager();
    return graphicManager->init_graphic_objects();
}


void cleanup(){
    mainWindow.close();
    if(graphicManager != nullptr){
        delete graphicManager;
        graphicManager = nullptr;
    }
 }

void refreshDrawedObjects(){
        mainWindow.clear(sf::Color(230, 230, 230));
        mainWindow.draw(graphicManager->getGuideText());
        mainWindow.draw(graphicManager->getRegisterText());
        mainWindow.draw(graphicManager->getInstructionText1());
        mainWindow.draw(graphicManager->getInstructionText2());
        mainWindow.draw(graphicManager->getInstructionText4());
        mainWindow.draw(graphicManager->getInstructionText5());
        mainWindow.draw(graphicManager->getRequirementsText0());
        mainWindow.draw(graphicManager->getRequirementsText1());
        mainWindow.draw(graphicManager->getRequirementsText2());
        mainWindow.draw(graphicManager->getEmailLabel());
        mainWindow.draw(graphicManager->getEmailText());
        mainWindow.draw(graphicManager->getEmailDomain());
        mainWindow.draw(graphicManager->getCodeLabel());
        mainWindow.draw(graphicManager->getCodeText());
        mainWindow.draw(graphicManager->getEmailInputText());
        mainWindow.draw(graphicManager->getCodeInputText());
        mainWindow.draw(graphicManager->getCursorEmailPointer());
        mainWindow.draw(graphicManager->getCursorCodePointer());
        mainWindow.draw(graphicManager->getArrowLinePointer());
        mainWindow.draw(graphicManager->getEmailApprovalLabel());
        mainWindow.draw(graphicManager->getCodeApprovalLabel());
        mainWindow.display();
 }
