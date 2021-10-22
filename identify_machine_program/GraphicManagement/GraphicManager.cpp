#include <SFML/Graphics.hpp>
#include "GraphicManager.h"

GraphicManager::GraphicManager(){}

std::string GraphicManager::getFormatErrorInfo(){
    return formatErrorInfo;
}

sf::RectangleShape& GraphicManager::getEmailLabel(){
    return emailLabel;
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

sf::Text& GraphicManager::getEmailText(){
    return emailText;
}

sf::Text& GraphicManager::getEmailInputText(){
    return emailInputText;
}

sf::Text& GraphicManager::getEmailDomain(){
    return emailDomain;
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

sf::Text& GraphicManager::getCursorEmailPointer(){
    return cursorEmailPointer;
}

sf::Text& GraphicManager::getCursorCodePointer(){
    return cursorCodePointer;
}

sf::Text& GraphicManager::getArrowLinePointer(){
    return arrowLinePointer;
}

 sf::RectangleShape& GraphicManager::getEmailApprovalLabel(){
    return emailApprovalLabel;
 }

 sf::RectangleShape& GraphicManager::getCodeApprovalLabel(){
    return codeApprovalLabel;
 }

int GraphicManager::init_graphic_objects(){

    if (!font.loadFromFile(fontPath+"arial.ttf") || !calibriFont.loadFromFile(fontPath+"CalibriRegular.ttf"))
        return -1;

    //guideText - Informacyjny naglowek do instrukcji korzystania
    guideText.setPosition(5,150);
    guideText.setFillColor(sf::Color(0, 0, 0));

    instructionText1.setPosition(5,200);
    instructionText1.setFillColor(sf::Color(0, 0, 0));

    instructionText2.setPosition(5,250);
    instructionText2.setFillColor(sf::Color(0, 0, 0));

    instructionText4.setPosition(5,300);
    instructionText4.setFillColor(sf::Color(0, 0, 0));

    instructionText5.setPosition(5,350);
    instructionText5.setFillColor(sf::Color(0, 0, 0));

    requirementsText0.setPosition(5,400);
    requirementsText0.setFillColor(sf::Color(0, 0, 0));

    requirementsText1.setPosition(5,450);
    requirementsText1.setFillColor(sf::Color(0, 0, 0));

    requirementsText2.setPosition(5,500);
    requirementsText2.setFillColor(sf::Color(0, 0, 0));

    registerText.setPosition(155,580);
    registerText.setFillColor(sf::Color(0, 0, 0));

    errorText.setPosition(5,20);
    errorText.setFillColor(sf::Color(0,0,0));

    errorGoBackText.setPosition(120,400);
    errorGoBackText.setFillColor(sf::Color(0,0,0));

    confirmationText.setPosition(70,80);
    confirmationText.setFillColor(sf::Color(0,0,0));

    confirmInputText.setPosition(289,80);
    confirmInputText.setFillColor(sf::Color(0,0,0));

    confirmEnterText.setPosition(110,140);
    confirmEnterText.setFillColor(sf::Color(0,0,0));

    //emailLabel - Obramowanie etykiety do emaila
    emailLabel.setOutlineThickness(2.0);
    emailLabel.setPosition(5,40);
    emailLabel.setFillColor(sf::Color(255, 255, 255));
    emailLabel.setOutlineColor(sf::Color(0, 0, 0));
    //emailText - etykieta do email
    emailText.setFillColor(sf::Color(0, 0, 0));
    emailText.setPosition(10,40);
    //emailInputText - input - emailu
    emailInputText.setFillColor(sf::Color(0, 0, 0));
    emailInputText.setPosition(70,40);
    emailInputText.setLineSpacing(-1);
    
    //emailDomain - text
    emailDomain.setFillColor(sf::Color(0, 0, 0));
    emailDomain.setPosition(166,40);

    //codeLabel - Obramowanie etykiety do unikatowego kodu studenta
    codeLabel.setOutlineThickness(2.0);
    codeLabel.setPosition(5,90);
    codeLabel.setFillColor(sf::Color(255, 255, 255));
    codeLabel.setOutlineColor(sf::Color(0, 0, 0));
    //codeText - etykieta do unikatowego kodu studenta
    codeText.setFillColor(sf::Color(0, 0, 0));
    codeText.setPosition(10,90);
    //codeInputText - input - nr emailu
    codeInputText.setFillColor(sf::Color(0, 0, 0));
    codeInputText.setPosition(165,90);
    codeInputText.setLineSpacing(-1);

    //cursorEmailPointer - kursor do nr emailu
    cursorEmailPointer.setFillColor(sf::Color(0,0,0));
    cursorEmailPointer.setPosition(70,40);
    cursorEmailPointer.setLetterSpacing(2.8);

    //cursorCodePointer - kursor do unikatowego kodu
    cursorCodePointer.setFillColor(sf::Color(0,0,0));
    cursorCodePointer.setPosition(165,90);
    cursorCodePointer.setLetterSpacing(2.8);

    arrowLinePointer.setFillColor(sf::Color(0,0,0));
    arrowLinePointer.setPosition(300,41);

    //emailApprovalLabel - Obramowanie etykiety dla konfirmacji emailu studenta
    emailApprovalLabel.setOutlineThickness(2.0);
    emailApprovalLabel.setPosition(400,40);
    emailApprovalLabel.setFillColor(sf::Color(255, 5, 0));
    emailApprovalLabel.setOutlineColor(sf::Color(0, 0, 0));

    //codeApprovalLabel - Obramowanie etykiety dla konfirmacji kodu studenta
    codeApprovalLabel.setOutlineThickness(2.0);
    codeApprovalLabel.setPosition(400,90);
    codeApprovalLabel.setFillColor(sf::Color(255, 5, 0));
    codeApprovalLabel.setOutlineColor(sf::Color(0, 0, 0));

    return 0;
}

