#include <SFML/Graphics.hpp>
#include <string>

class GraphicManager {

private:
    const std::string fontPath = "/home/cerber/Documents/lab_supervision/identify_machine_program/";
    sf::Font font;
    sf::Font calibriFont;
    std::string formatErrorInfo = "Format of the input is wrong. Registration did not succeed."; 
    sf::RectangleShape emailLabel = sf::RectangleShape(sf::Vector2f(280, 30));
    sf::Text registerText = sf::Text("Register (ENTER)",font,26);
    sf::Text guideText = sf::Text(" I. User manual:",font, 22);
    sf::Text instructionText1 = sf::Text("- Use your '8 digits' email version, you don't have to put email domain.",calibriFont,18);
    sf::Text instructionText2 = sf::Text("- Use TAB key to move between 'Email' and 'Code/Password' input field",calibriFont,18);
    sf::Text instructionText4 = sf::Text("- Use BACKSPACE key to remove input from chosen field",calibriFont,18);
    sf::Text instructionText5 = sf::Text("- Use ENTER key to confirm given credentials",calibriFont,18);
    sf::Text requirementsText0 = sf::Text("II. Requirements: ",font, 22);
    sf::Text requirementsText1 = sf::Text("- Both 'Email' and 'Code/Password' are required",calibriFont,18);
    sf::Text requirementsText2 = sf::Text("- Green square indicates that input format is correct",calibriFont,18);
    sf::Text emailText = sf::Text("Email: ", font, 20);
    sf::Text emailInputText = sf::Text("", font, 20);
    sf::Text emailDomain = sf::Text("@pw.edu.pl",font,20);
    sf::Text errorText = sf::Text("",calibriFont,20);
    sf::Text errorGoBackText = sf::Text("Continue (ENTER)",font,26);
    sf::Text confirmationText = sf::Text("Are you sure? (y/n): [   ]",calibriFont,25);
    sf::Text confirmEnterText = sf::Text("Confirm (Enter)",calibriFont,30);
    sf::Text confirmInputText = sf::Text("", calibriFont, 25);
    sf::RectangleShape codeLabel = sf::RectangleShape(sf::Vector2f(330, 30));
    sf::Text codeText = sf::Text("Code/Password: ", font, 20);
    sf::Text codeInputText = sf::Text("", font, 20);
    sf::Text cursorEmailPointer = sf::Text("_", font, 20);
    sf::Text cursorCodePointer = sf::Text(" _ ", font, 20);
    sf::Text arrowLinePointer = sf::Text("<",font,20);
    sf::RectangleShape emailApprovalLabel = sf::RectangleShape(sf::Vector2f(30, 30));
    sf::RectangleShape codeApprovalLabel = sf::RectangleShape(sf::Vector2f(30, 30));
    sf::Text emailToConfirm = sf::Text("", font, 20);
    sf::Text codeToConfirm = sf::Text("",font,20);
public:
    GraphicManager();
    int init_graphic_objects();
    std::string getFormatErrorInfo();
    sf::RectangleShape& getEmailLabel();
    sf::Text& getRegisterText();
    sf::Text& getGuideText();
    sf::Text& getInstructionText1();
    sf::Text& getInstructionText2();
    sf::Text& getInstructionText4();
    sf::Text& getInstructionText5();
    sf::Text& getRequirementsText0();
    sf::Text& getRequirementsText1();
    sf::Text& getRequirementsText2();
    sf::Text& getEmailText();
    sf::Text& getEmailInputText();
    sf::Text& getEmailToConfirm();
    sf::Text& getCodeToConfirm();
    sf::Text& getErrorText();
    sf::Text& getErrorGoBackText();
    sf::Text& getConfirmationText();
    sf::Text& getConfirmEnterText();
    sf::Text& getConfirmInputText();
    sf::RectangleShape& getCodeLabel();
    sf::Text& getCodeText();
    sf::Text& getCodeInputText();
    sf::Text& getCursorEmailPointer();
    sf::Text& getCursorCodePointer();
    sf::Text& getArrowLinePointer();
    sf::Text& getEmailDomain();
    sf::RectangleShape& getEmailApprovalLabel();
    sf::RectangleShape& getCodeApprovalLabel();
};