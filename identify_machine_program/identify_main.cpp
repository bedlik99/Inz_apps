#include <SFML/Graphics.hpp>
#include <SFML/Window.hpp>
#include <SFML/System.hpp>
#include <sys/ptrace.h>
#include <iostream>
#include <string>
#include <cmath>
#include "GraphicManager.h"
#include "RESTConnector/RestServerConnector.h"
#include "RESTConnector/IOConfig/IOConfig.h"

void blinkChosenLine();
void waitForBlink();
void continueExecution();
void endExecution(int status);
void refreshDrawedObjects();
void handleInfoWindow(std::string errorInfo);
int handleApprovalWindow();
int init();
void cleanup();

static sf::RenderWindow mainWindow(sf::VideoMode(600, 700), "Registration",sf::Style::Titlebar);
static sf::String indexInput;
static sf::String codeInput;  
static sf::String tmpIndexCursor="_";
static sf::String tmpCodeCursor="_";  
static bool isIndex = true;
static bool secondHasElapsed = true;
static GraphicManager* graphicManager = new GraphicManager();
static RestServerConnector* restServerConnector = new RestServerConnector();
static IOConfig ioConfig;
static sf::Clock customClock;
static sf::Mutex blinkMutex;
static __INT64_TYPE__ firstTime=0;

void continueExecution(){}
void endExecution(int status){
    cleanup();
    kill(getpid(), SIGTERM);
    exit(0);
}

int main() {

    ptrace(PTRACE_TRACEME, 0, 0, 0) < 0 ? endExecution(0) : continueExecution();
    init() != 0 ? endExecution(-1) : continueExecution();

    int ASCII_DEC_CODE = 0; 
    int restCommunicationReturnCode = 0;
    while (mainWindow.isOpen()) {
        sf::Event event;

        if(abs(customClock.getElapsedTime().asMilliseconds() - firstTime) > 500){
            sf::Thread thread(&waitForBlink);
            thread.launch();
        }
      
        while (mainWindow.pollEvent(event) ) {
        
            switch (event.type){
                case sf::Event::TextEntered: 
                     ASCII_DEC_CODE = static_cast<int>(event.key.code);

                        if(ASCII_DEC_CODE == 27){
                            if(handleApprovalWindow()==1)
                            mainWindow.close();
                        }
 
                        if(ASCII_DEC_CODE == 13){
                            if(indexInput.getSize()!=6 || codeInput.getSize()!=6){
                                handleInfoWindow(graphicManager->getFormatErrorInfo());
                                indexInput="";
                                codeInput="";
                                graphicManager->getIndexInputText().setString(indexInput);
                                graphicManager->getCodeInputText().setString(codeInput);
                                tmpIndexCursor="_";
                                tmpCodeCursor="_";
                                graphicManager->getCursorCodePointer().setString(tmpCodeCursor);
                                graphicManager->getCursorIndexPointer().setString(tmpIndexCursor);
                                refreshDrawedObjects();
                            }else{
                                if(handleApprovalWindow()==1){
                                    restCommunicationReturnCode = restServerConnector->sendData(static_cast<std::string>(indexInput),static_cast<std::string>(codeInput));
                                    switch (restCommunicationReturnCode){
                                        case 200:
                                            handleInfoWindow(graphicManager->getSuccesfullResponseInfo().append(std::to_string(restCommunicationReturnCode)));
                                            mainWindow.close();
                                            exit(0);
                                        case -5:
                                            handleInfoWindow(graphicManager->getNoConnectionErrorInfo());
                                            break;              
                                        default :
                                            handleInfoWindow(graphicManager->getServerErrorInfo().append(std::to_string(restCommunicationReturnCode)));
                                            break;
                                    }


                                }
                            }

                        }

                        if(ASCII_DEC_CODE == 9){
                            isIndex = !isIndex;
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

void handleInfoWindow(std::string errorInfo){
    sf::RenderWindow errorWindow(sf::VideoMode(650, 500), "Error",sf::Style::Titlebar);
    graphicManager->getErrorText().setString(errorInfo);
    errorWindow.setPosition(sf::Vector2i(2, 2));
    errorWindow.requestFocus();
    mainWindow.setVisible(false);

    while (errorWindow.isOpen()) {
        sf::Event event;
        while (errorWindow.pollEvent(event)) {
                switch (event.type){
                    case sf::Event::TextEntered:
                        if(static_cast<int>(event.key.code) == 13){
                            errorWindow.close();
                        }
                        break;
                 }

        }
        errorWindow.clear(sf::Color(230, 230, 230));
        errorWindow.draw(graphicManager->getErrorText());
        errorWindow.draw(graphicManager->getErrorGoBackText());
        errorWindow.display();
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
    confirmationWindow.requestFocus();
    mainWindow.setVisible(false);

    while (confirmationWindow.isOpen()) {
        sf::Event event;
        while (confirmationWindow.pollEvent(event)) {
                switch (event.type){

                    case sf::Event::TextEntered:
                        ASCII_DEC_CODE = static_cast<int>(event.key.code);

                        if(ASCII_DEC_CODE == 121 || ASCII_DEC_CODE == 110 || ASCII_DEC_CODE ==78 || ASCII_DEC_CODE == 89){  
                            return_code = (ASCII_DEC_CODE == 121 || ASCII_DEC_CODE == 89) ? 1 : 0;
                            graphicManager->getConfirmInputText().setString(event.text.unicode);
                            confirmationWindow.clear(sf::Color(230, 230, 230));
                            confirmationWindow.draw(graphicManager->getConfirmationText());
                            confirmationWindow.draw(graphicManager->getConfirmInputText());
                            confirmationWindow.display();
                        }
                        if(ASCII_DEC_CODE == 13 && !graphicManager->getConfirmInputText().getString().isEmpty()){
                            confirmationWindow.close();
                        }
                        break;
                }
        }
        confirmationWindow.clear(sf::Color(230, 230, 230));
        confirmationWindow.draw(graphicManager->getConfirmationText());
        confirmationWindow.draw(graphicManager->getConfirmInputText());
        confirmationWindow.draw(graphicManager->getConfirmEnterText());
        confirmationWindow.display();
    }
    mainWindow.setPosition(sf::Vector2i(2, 2));
    mainWindow.setVisible(true);

    return return_code;
}

void waitForBlink(){
        blinkMutex.lock();
        sf::Thread thread(&blinkChosenLine);
        thread.launch();
        blinkMutex.unlock();
}

void blinkChosenLine(){
        blinkMutex.lock();

        if(secondHasElapsed && abs(customClock.getElapsedTime().asMilliseconds() - firstTime) > 1200) {

            if(isIndex){
                graphicManager->getCursorIndexPointer().setColor(sf::Color(255, 255, 255));
                refreshDrawedObjects();
                firstTime = customClock.restart().asMilliseconds();
            }else{
                graphicManager->getCursorCodePointer().setColor(sf::Color(255, 255, 255));
                refreshDrawedObjects();
                firstTime = customClock.restart().asMilliseconds();           
            }
            secondHasElapsed = false;
        }

        if(!secondHasElapsed && abs(customClock.getElapsedTime().asMilliseconds() - firstTime) > 800) {
            if(isIndex){
                graphicManager->getCursorIndexPointer().setColor(sf::Color(0, 0, 0));
                refreshDrawedObjects();
            }else{
                graphicManager->getCursorCodePointer().setColor(sf::Color(0, 0, 0));
                refreshDrawedObjects();
            }
            secondHasElapsed = true;
        }
        blinkMutex.unlock();
}   

int init(){
    if(ioConfig.doesFileExist(restServerConnector->getRegistrationFilePath()) 
    && !ioConfig.isSecretFileEmpty(restServerConnector->getRegistrationFilePath())){
            return -1;
    }
    mainWindow.setPosition(sf::Vector2i(2, 2));
    return graphicManager->init_graphic_objects();
}

void cleanup(){
    mainWindow.close();
    delete graphicManager;
    delete restServerConnector;
 }

void refreshDrawedObjects(){
        mainWindow.clear(sf::Color(230, 230, 230));
        mainWindow.draw(graphicManager->getGuideText());
        mainWindow.draw(graphicManager->getRegisterText());
        mainWindow.draw(graphicManager->getInstructionText1());
        mainWindow.draw(graphicManager->getInstructionText2());
        mainWindow.draw(graphicManager->getInstructionText3());
        mainWindow.draw(graphicManager->getInstructionText4());
        mainWindow.draw(graphicManager->getInstructionText5());
        mainWindow.draw(graphicManager->getRequirementsText0());
        mainWindow.draw(graphicManager->getRequirementsText1());
        mainWindow.draw(graphicManager->getRequirementsText2());
        mainWindow.draw(graphicManager->getQuitConsoleText());
        mainWindow.draw(graphicManager->getIndexLabel());
        mainWindow.draw(graphicManager->getIndexText());
        mainWindow.draw(graphicManager->getCodeLabel());
        mainWindow.draw(graphicManager->getCodeText());
        mainWindow.draw(graphicManager->getIndexInputText());
        mainWindow.draw(graphicManager->getCodeInputText());
        mainWindow.draw(graphicManager->getCursorIndexPointer());
        mainWindow.draw(graphicManager->getCursorCodePointer());
        mainWindow.display();
 }
