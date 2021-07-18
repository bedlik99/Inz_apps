#include <SFML/Graphics.hpp>
#include "GraphicManager.h"

GraphicManager::GraphicManager(){}

std::string GraphicManager::getSuccesfullResponseInfo(){
    return succesfullResponseInfo;
}

std::string GraphicManager::getNoConnectionErrorInfo(){
    return noConnectionErrorInfo;
}

std::string GraphicManager::getFormatErrorInfo(){
    return formatErrorInfo;
}

std::string GraphicManager::getServerErrorInfo(){
    return serverErrorInfo;
}

sf::RectangleShape& GraphicManager::getIndexLabel(){
    return indexLabel;
}

sf::Text& GraphicManager::getQuitConsoleText(){
    return quitConsoleText;
}

sf::Text& GraphicManager::getRegisterText(){
    return registerText;
}

sf::Text& GraphicManager::getGuideText(){
    return guideText;
}

sf::Text& GraphicManager::getInstructionText1(){
    return instructionText1;
}

sf::Text& GraphicManager::getInstructionText2(){
    return instructionText2;
}

sf::Text& GraphicManager::getInstructionText3(){
    return instructionText3;
}

sf::Text& GraphicManager::getInstructionText4(){
    return instructionText4;
}

sf::Text& GraphicManager::getInstructionText5(){
    return instructionText5;
}

sf::Text& GraphicManager::getRequirementsText0(){
    return requirementsText0;
}

sf::Text& GraphicManager::getRequirementsText1(){
    return requirementsText1;
}

sf::Text& GraphicManager::getRequirementsText2(){
    return requirementsText2;
}

sf::Text& GraphicManager::getIndexText(){
    return indexText;
}

sf::Text& GraphicManager::getIndexInputText(){
    return indexInputText;
}

sf::Text& GraphicManager::getErrorText(){
    return errorText;
}

sf::Text& GraphicManager::getErrorGoBackText(){
    return errorGoBackText;
}

sf::Text& GraphicManager::getConfirmationText(){
    return confirmationText;
}

sf::Text& GraphicManager::getConfirmEnterText(){
    return confirmEnterText;
}

sf::Text& GraphicManager::getConfirmInputText(){
    return confirmInputText;
}

sf::RectangleShape& GraphicManager::getCodeLabel(){
    return codeLabel;
}

sf::Text& GraphicManager::getCodeText(){
    return codeText;
}

sf::Text& GraphicManager::getCodeInputText(){
    return codeInputText;
}

sf::Text& GraphicManager::getCursorIndexPointer(){
    return cursorIndexPointer;
}

sf::Text& GraphicManager::getCursorCodePointer(){
    return cursorCodePointer;
}

sf::Text& GraphicManager::getArrowLinePointer(){
    return arrowLinePointer;
}

int GraphicManager::init_graphic_objects(){

    if (!font.loadFromFile("arial.ttf") || !calibriFont.loadFromFile("CalibriRegular.ttf"))
        return -1;

    //guideText - Informacyjny naglowek do instrukcji korzystania
    guideText.setPosition(5,150);
    guideText.setColor(sf::Color(0, 0, 0));

    instructionText1.setPosition(5,200);
    instructionText1.setColor(sf::Color(0, 0, 0));

    instructionText2.setPosition(5,250);
    instructionText2.setColor(sf::Color(0, 0, 0));

    instructionText3.setPosition(5,300);
    instructionText3.setColor(sf::Color(0, 0, 0));

    instructionText4.setPosition(5,350);
    instructionText4.setColor(sf::Color(0, 0, 0));

    instructionText5.setPosition(5,400);
    instructionText5.setColor(sf::Color(0, 0, 0));

    requirementsText0.setPosition(5,450);
    requirementsText0.setColor(sf::Color(0, 0, 0));

    requirementsText1.setPosition(5,500);
    requirementsText1.setColor(sf::Color(0, 0, 0));

    requirementsText2.setPosition(5,550);
    requirementsText2.setColor(sf::Color(0, 0, 0));

    registerText.setPosition(200,650);
    registerText.setColor(sf::Color(0, 0, 0));

    quitConsoleText.setPosition(505,2);
    quitConsoleText.setColor(sf::Color(0, 0, 0));

    errorText.setPosition(5,20);
    errorText.setColor(sf::Color(0,0,0));

    errorGoBackText.setPosition(120,400);
    errorGoBackText.setColor(sf::Color(0,0,0));

    confirmationText.setPosition(70,80);
    confirmationText.setColor(sf::Color(0,0,0));

    confirmInputText.setPosition(289,80);
    confirmInputText.setColor(sf::Color(0,0,0));

    confirmEnterText.setPosition(110,140);
    confirmEnterText.setColor(sf::Color(0,0,0));

    //indexLabel - Obramowanie etykiety do nr_indeksu
    indexLabel.setOutlineThickness(2.0);
    indexLabel.setPosition(5,40);
    indexLabel.setFillColor(sf::Color(255, 255, 255));
    indexLabel.setOutlineColor(sf::Color(0, 0, 0));
    //indexText - etykieta do nr indeks
    indexText.setColor(sf::Color(0, 0, 0));
    indexText.setPosition(10,40);
    //indexInputText - input - nr indeksu
    indexInputText.setColor(sf::Color(0, 0, 0));
    indexInputText.setPosition(150,40);
    indexInputText.setLineSpacing(-1);

    //codeLabel - Obramowanie etykiety do unikatowego kodu studenta
    codeLabel.setOutlineThickness(2.0);
    codeLabel.setPosition(5,90);
    codeLabel.setFillColor(sf::Color(255, 255, 255));
    codeLabel.setOutlineColor(sf::Color(0, 0, 0));
    //codeText - etykieta do unikatowego kodu studenta
    codeText.setColor(sf::Color(0, 0, 0));
    codeText.setPosition(10,90);
    //codeInputText - input - nr indeksu
    codeInputText.setColor(sf::Color(0, 0, 0));
    codeInputText.setPosition(150,90);
    codeInputText.setLineSpacing(-1);

    //cursorIndexPointer
    cursorIndexPointer.setColor(sf::Color(0,0,0));
    cursorIndexPointer.setPosition(150,40);
    cursorIndexPointer.setLetterSpacing(2.8);

    //cursorCodePointer
    cursorCodePointer.setColor(sf::Color(255,255,255));
    cursorCodePointer.setPosition(150,90);
    cursorCodePointer.setLetterSpacing(2.8);

    arrowLinePointer.setColor(sf::Color(0,0,0));
    arrowLinePointer.setPosition(90,40);

    return 0;
}

