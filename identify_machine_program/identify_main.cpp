#include <SFML/Graphics.hpp>
#include <SFML/Window.hpp>
#include <SFML/System.hpp>
#include <iostream>
#include <string>
#include <cmath>

void blinkChosenLine();
void waitForBlink();
void refreshDrawedObjects();

static sf::RenderWindow window(sf::VideoMode(600, 700), "Register machine",sf::Style::Titlebar);
static std::string tmpString;
static sf::String indexInput;
static sf::String codeInput;  
static sf::String tmpIndexCursor="_";
static sf::String tmpCodeCursor="_";  
static sf::Font font;
static sf::Font calibriFont; 
static bool isIndex = true;
static bool secondHasElapsed = true;
static int ASCII_DEC_CODE; 
static sf::RectangleShape indexLabel(sf::Vector2f(300, 30));
static sf::Text quitConsoleText("Cancel(ESC)",calibriFont,17);
static sf::Text registerText("Send data(ENTER)",calibriFont,19);
static sf::Text headerText("Send credentials",font, 30);
static sf::Text guideText(" I. User manual:",font, 22);
static sf::Text instructionText1(" Use only keyboard keys to communicate with GUI",calibriFont,20);
static sf::Text instructionText2("- Use TAB key to move between 'Index' nr' and 'Code' input field",calibriFont,17);
static sf::Text instructionText3("- Use ESC key to cancel registration of the machine if You registered it before",calibriFont,17);
static sf::Text instructionText4("- Use ENTER key to send given credentials",calibriFont,17);
static sf::Text instructionText5("- Use BACKSPACE key to remove input from input fields",calibriFont,17);
static sf::Text requirementsText0("II. Requirements: ",font, 22);
static sf::Text requirementsText1("* Both 'Index nr' and 'Code' are required",calibriFont,17);
static sf::Text requirementsText2("* You can register this machine only once",calibriFont,17);
static sf::Text indexText("Index nr: ", font, 20);
static sf::Text indexInputText("", font, 20);
static sf::RectangleShape codeLabel(sf::Vector2f(300, 30));
static sf::Text codeText("Code: ", font, 20);
static sf::Text codeInputText("", font, 20);
static sf::Text cursorIndexPointer("_", font, 20);
static sf::Text cursorCodePointer("_", font, 20);
static sf::Clock customClock = sf::Clock();
static sf::Mutex blinkMutex;
static __INT64_TYPE__ firstTime=0;

int main() {

    if (!font.loadFromFile("arial.ttf"))
        return EXIT_FAILURE;
    if (!calibriFont.loadFromFile("CalibriRegular.ttf"))
        return EXIT_FAILURE;
        
    //headerText - Informacyjny naglowek okna
    headerText.setPosition(175,2);
    headerText.setColor(sf::Color(255, 255, 255));

    //guideText - Informacyjny naglowek do instrukcji korzystania
    guideText.setPosition(5,150);
    guideText.setColor(sf::Color(255, 255, 255));

    instructionText1.setPosition(5,200);
    instructionText1.setColor(sf::Color(255, 255, 255));

    instructionText2.setPosition(5,250);
    instructionText2.setColor(sf::Color(255, 255, 255));

    instructionText3.setPosition(5,300);
    instructionText3.setColor(sf::Color(255, 255, 255));

    instructionText4.setPosition(5,350);
    instructionText4.setColor(sf::Color(255, 255, 255));

    instructionText5.setPosition(5,400);
    instructionText5.setColor(sf::Color(255, 255, 255));

    requirementsText0.setPosition(5,450);
    requirementsText0.setColor(sf::Color(255, 255, 255));

    requirementsText1.setPosition(5,500);
    requirementsText1.setColor(sf::Color(255, 255, 255));

    requirementsText2.setPosition(5,550);
    requirementsText2.setColor(sf::Color(255, 255, 255));

    registerText.setPosition(220,600);
    registerText.setColor(sf::Color(249, 252, 25));

    quitConsoleText.setPosition(520,2);
    quitConsoleText.setColor(sf::Color(255, 255, 255));

    //indexLabel - Obramowanie etykiety do nr_indeksu
    indexLabel.setOutlineThickness(2.0);
    indexLabel.setPosition(5,50);
    indexLabel.setFillColor(sf::Color(0, 0, 0));
    indexLabel.setOutlineColor(sf::Color(0, 0, 0));
    //indexText - etykieta do nr indeks
    indexText.setColor(sf::Color(36, 191, 75));
    indexText.setPosition(10,50);
    //indexInputText - input - nr indeksu
    indexInputText.setColor(sf::Color(255, 255, 255));
    indexInputText.setPosition(150,50);
    indexInputText.setLineSpacing(-1);

    //codeLabel - Obramowanie etykiety do unikatowego kodu studenta
    codeLabel.setOutlineThickness(2.0);
    codeLabel.setPosition(5,100);
    codeLabel.setFillColor(sf::Color(0, 0, 0));
    codeLabel.setOutlineColor(sf::Color(0, 0, 0));
    //codeText - etykieta do unikatowego kodu studenta
    codeText.setColor(sf::Color(36, 191, 75));
    codeText.setPosition(10,100);
    //codeInputText - input - nr indeksu
    codeInputText.setColor(sf::Color(255, 255, 255));
    codeInputText.setPosition(150,100);
    codeInputText.setLineSpacing(-1);

    //cursorIndexPointer
    cursorIndexPointer.setColor(sf::Color(255, 255, 255));
    cursorIndexPointer.setPosition(150,50);
    cursorIndexPointer.setLetterSpacing(2.8);

    //cursorCodePointer
    cursorCodePointer.setColor(sf::Color(255, 255, 255));
    cursorCodePointer.setPosition(150,100);
    cursorCodePointer.setLetterSpacing(2.8);

    while (window.isOpen()) {
        sf::Event event;

        if(abs(customClock.getElapsedTime().asMilliseconds() - firstTime) > 500){
            sf::Thread thread(&waitForBlink);
            thread.launch();
        }
      
        while (window.pollEvent(event)) {
        
            switch (event.type){

                case sf::Event::Closed:
                    window.close();
                    break;

                case sf::Event::TextEntered:
                    ASCII_DEC_CODE = static_cast<int>(event.key.code);

                    if(ASCII_DEC_CODE == 27){
                        window.close();
                    }

                    if(ASCII_DEC_CODE == 13){
                        if(indexInput.getSize()!=6 || codeInput.getSize()!=6){
                            std::cout << "Wrong format of credentials" << std::endl;
                        }else{

                            std::cout << "Sending data..." << std::endl;

                        }
                        

                    }

                    if(ASCII_DEC_CODE == 9){
                        isIndex = !isIndex;
                    }

                    if(isIndex && ((ASCII_DEC_CODE >=48 && ASCII_DEC_CODE <=57) || ASCII_DEC_CODE==8)) {
                        if(ASCII_DEC_CODE != 8 && indexInput.getSize()<6) {
                             indexInput += event.text.unicode;
                             tmpIndexCursor.insert(0," ");
                             cursorIndexPointer.setString(tmpIndexCursor);
                             indexInputText.setString(indexInput);
                             refreshDrawedObjects();

                        }else if(indexInput.getSize() > 0 && ASCII_DEC_CODE == 8){
                             indexInput = indexInput.substring(0,indexInput.getSize()-1);
                             tmpIndexCursor = tmpIndexCursor.substring(1);
                             cursorIndexPointer.setString(tmpIndexCursor);
                             indexInputText.setString(indexInput);
                             refreshDrawedObjects();
                        }
                      
                    }else if(!isIndex && ((ASCII_DEC_CODE >=33 && ASCII_DEC_CODE <=126) || ASCII_DEC_CODE==8)){
                        if(ASCII_DEC_CODE != 8 && codeInput.getSize()<6){
                            codeInput += event.text.unicode;
                            tmpCodeCursor.insert(0," ");
                            cursorCodePointer.setString(tmpCodeCursor);
                            codeInputText.setString(codeInput);

                            refreshDrawedObjects();;   

                        }else if (codeInput.getSize() > 0 && ASCII_DEC_CODE == 8){
                         codeInput = codeInput.substring(0, codeInput.getSize()-1);
                            tmpCodeCursor = tmpCodeCursor.substring(1);
                            cursorCodePointer.setString(tmpCodeCursor);
                            codeInputText.setString(codeInput);   

                            refreshDrawedObjects();
                        }
                    
                    }
                    break;  
            }

        refreshDrawedObjects();
        }
    }
    return 0;
}

void waitForBlink(){
        blinkMutex.lock();
        sf::Thread thread(&blinkChosenLine);
        thread.launch();
        blinkMutex.unlock();
}

void blinkChosenLine(){
        blinkMutex.lock();

        if(secondHasElapsed && abs(customClock.getElapsedTime().asMilliseconds() - firstTime) > 1500) {

            if(isIndex){
                cursorIndexPointer.setColor(sf::Color(0, 0, 0));

                refreshDrawedObjects();
                firstTime = customClock.restart().asMilliseconds();
            }else{
                cursorCodePointer.setColor(sf::Color(0, 0, 0));

                refreshDrawedObjects();
                firstTime = customClock.restart().asMilliseconds();
                
            }
            secondHasElapsed = false;
        }

        if(!secondHasElapsed && abs(customClock.getElapsedTime().asMilliseconds() - firstTime) > 1000) {
            if(isIndex){
                cursorIndexPointer.setColor(sf::Color(255, 255, 255));

                refreshDrawedObjects();
            }else{
                cursorCodePointer.setColor(sf::Color(255, 255, 255));

                refreshDrawedObjects();
            }
            secondHasElapsed = true;
        }
        blinkMutex.unlock();
}   

void refreshDrawedObjects(){
        window.draw(guideText);
        window.draw(registerText);
        window.draw(instructionText1);
        window.draw(instructionText2);
        window.draw(instructionText3);
        window.draw(instructionText4);
        window.draw(instructionText5);
        window.draw(requirementsText0);
        window.draw(requirementsText1);
        window.draw(requirementsText2);
        window.draw(quitConsoleText);
        window.draw(headerText);
        window.draw(indexLabel);
        window.draw(indexText);
        window.draw(codeLabel);
        window.draw(codeText);
        window.draw(indexInputText);
        window.draw(codeInputText);
        window.draw(cursorIndexPointer);
        window.draw(cursorCodePointer);
        window.display();
 }


