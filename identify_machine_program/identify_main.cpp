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
void createRegisterLog(std::string indexInput, std::string codeInput);
void handleInfoWindow(std::string errorInfo,std::string windowTitle);
int handleApprovalWindow();
int init();
void cleanup();

const static std::string logsFilePath = "/etc/identify_lab_data/logs_dir/main_logs";
static sf::RenderWindow mainWindow(sf::VideoMode(500, 650), "Registration",sf::Style::Titlebar);
static sf::String indexInput;
static sf::String codeInput;  
static sf::String tmpIndexCursor="_";
static sf::String tmpCodeCursor="_";  
static bool isIndex = true;
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
                            if(indexInput.getSize()!=6 || codeInput.getSize()!=6){
                                if(indexInput.getSize()!=6 && codeInput.getSize()!=6){
                                    handleInfoWindow(graphicManager->getFormatErrorInfo(),"Error");
                                    indexInput="";
                                    codeInput="";
                                    graphicManager->getIndexInputText().setString(indexInput);
                                    graphicManager->getCodeInputText().setString(codeInput);
                                    tmpCodeCursor="_";
                                    tmpIndexCursor="_";
                                    graphicManager->getCursorCodePointer().setString(tmpCodeCursor);
                                    graphicManager->getCursorIndexPointer().setString(tmpIndexCursor);
                                    refreshDrawedObjects();
                                }else if(indexInput.getSize()!=6){
                                    handleInfoWindow(graphicManager->getFormatErrorInfo(),"Error");
                                    indexInput="";
                                    graphicManager->getIndexInputText().setString(indexInput);
                                    tmpIndexCursor="_";
                                    graphicManager->getCursorIndexPointer().setString(tmpIndexCursor);
                                    refreshDrawedObjects();
                                }else{
                                    handleInfoWindow(graphicManager->getFormatErrorInfo(),"Error");
                                    codeInput="";
                                    graphicManager->getCodeInputText().setString(codeInput);
                                    tmpCodeCursor="_";
                                    graphicManager->getCursorCodePointer().setString(tmpCodeCursor);
                                    refreshDrawedObjects();
                                }
                            }else{
                                if(handleApprovalWindow()==1){
                                    createRegisterLog(indexInput,codeInput);
                                    endExecution(0);
                                }
                            }
                        }

                        if(ASCII_DEC_CODE == 9){
                            isIndex = !isIndex;
                            if(isIndex){
                                graphicManager->getArrowLinePointer().setPosition(320,40);
                            }else{
                                graphicManager->getArrowLinePointer().setPosition(320,90);  
                            }
                            refreshDrawedObjects();
                        }

                        if(isIndex && ((ASCII_DEC_CODE >=48 && ASCII_DEC_CODE <=57) || ASCII_DEC_CODE==8)) {
                            if(ASCII_DEC_CODE != 8 && indexInput.getSize()<6) {
                                indexInput += event.text.unicode;
                                tmpIndexCursor.insert(0," ");
                                graphicManager->getCursorIndexPointer().setString(tmpIndexCursor);
                                graphicManager->getIndexInputText().setString(indexInput);
                                refreshDrawedObjects();

                            }else if(indexInput.getSize() > 0 && ASCII_DEC_CODE == 8){
                                indexInput = indexInput.substring(0,indexInput.getSize()-1);
                                tmpIndexCursor = tmpIndexCursor.substring(1);
                                graphicManager->getCursorIndexPointer().setString(tmpIndexCursor);
                                graphicManager->getIndexInputText().setString(indexInput);
                                refreshDrawedObjects();
                            }
                        
                        }else if(!isIndex && ((ASCII_DEC_CODE >=33 && ASCII_DEC_CODE <=126) || ASCII_DEC_CODE==8)){
                            if(ASCII_DEC_CODE != 8 && codeInput.getSize()<6){
                                codeInput += event.text.unicode;
                                tmpCodeCursor.insert(0," ");
                                graphicManager->getCursorCodePointer().setString(tmpCodeCursor);
                                graphicManager->getCodeInputText().setString(codeInput);
                                refreshDrawedObjects();  

                            }else if (codeInput.getSize() > 0 && ASCII_DEC_CODE == 8){
                            codeInput = codeInput.substring(0, codeInput.getSize()-1);
                                tmpCodeCursor = tmpCodeCursor.substring(1);
                                graphicManager->getCursorCodePointer().setString(tmpCodeCursor);
                                graphicManager->getCodeInputText().setString(codeInput);   
                                refreshDrawedObjects();
                            }
                        }
                    break;  
            }   
            refreshDrawedObjects();
        }
    }
    cleanup();
    return 0;
}

void createRegisterLog(std::string indexInput, std::string codeInput){
    if(ioConfig.trim(indexInput).length()==6 && ioConfig.trim(codeInput).length()==6){
        std::ofstream outputLogsFile;
        outputLogsFile.open(logsFilePath,std::ios::app);
        std::string registryInfo = indexInput + " " + codeInput;
        outputLogsFile << registryInfo << std::endl;
        outputLogsFile.close();
    }
}

void handleInfoWindow(std::string errorInfo, std::string windowTitle){
    sf::RenderWindow errorWindow(sf::VideoMode(650, 500), windowTitle,sf::Style::Titlebar);
    graphicManager->getErrorText().setString(errorInfo);
    errorWindow.setPosition(sf::Vector2i(2, 2));
    errorWindow.setKeyRepeatEnabled(false);
    errorWindow.setFramerateLimit(60);
    mainWindow.setVisible(false);

    while (errorWindow.isOpen()) {
        sf::Event event;
        errorWindow.clear(sf::Color(230, 230, 230));
        errorWindow.draw(graphicManager->getErrorText());
        errorWindow.draw(graphicManager->getErrorGoBackText());
        errorWindow.display();
        while (errorWindow.waitEvent(event)) {
            switch (event.type){
                case sf::Event::TextEntered:
                    if(static_cast<int>(event.key.code) == 13){
                        errorWindow.close();
                    }
                    break;
            }
            errorWindow.clear(sf::Color(230, 230, 230));
            errorWindow.draw(graphicManager->getErrorText());
            errorWindow.draw(graphicManager->getErrorGoBackText());
            errorWindow.display();         
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
    mainWindow.setVisible(true);

    return return_code;
}

int init(){
    mainWindow.setVisible(false);
    (ioConfig.isFileEmpty(logsFilePath) && !ioConfig.areCredentialsPresent()) ? continueExecutionV() : endExecution(0);
    sleep(8);
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
        mainWindow.draw(graphicManager->getIndexLabel());
        mainWindow.draw(graphicManager->getIndexText());
        mainWindow.draw(graphicManager->getCodeLabel());
        mainWindow.draw(graphicManager->getCodeText());
        mainWindow.draw(graphicManager->getIndexInputText());
        mainWindow.draw(graphicManager->getCodeInputText());
        mainWindow.draw(graphicManager->getCursorIndexPointer());
        mainWindow.draw(graphicManager->getCursorCodePointer());
        mainWindow.draw(graphicManager->getArrowLinePointer());
        mainWindow.display();
 }
