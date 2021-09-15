#include <SFML/Graphics.hpp>
#include <string>

class GraphicManager {

private:
    const std::string fontPath = "/home/cerber/Documents/inz_dyp/working_folder_inz/identify_machine_program/";
    sf::Font font;
    sf::Font calibriFont;
    std::string formatErrorInfo = "Format of the input is wrong. Registration do not succeed."; 
    sf::RectangleShape indexLabel = sf::RectangleShape(sf::Vector2f(300, 30));
    sf::Text registerText = sf::Text("Register (ENTER)",font,26);
    sf::Text guideText = sf::Text(" I. User manual:",font, 22);
    sf::Text instructionText1 = sf::Text(" Use only keyboard keys to communicate with GUI",calibriFont,20);
    sf::Text instructionText2 = sf::Text("- Use TAB key to move between 'Index' nr' and 'Code' input field",calibriFont,18);
    sf::Text instructionText4 = sf::Text("- Use ENTER key to send given credentials",calibriFont,18);
    sf::Text instructionText5 = sf::Text("- Use BACKSPACE key to remove input from input fields",calibriFont,18);
    sf::Text requirementsText0 = sf::Text("II. Requirements: ",font, 22);
    sf::Text requirementsText1 = sf::Text("* Both 'Index nr' and 'Code' are required",calibriFont,18);
    sf::Text indexText = sf::Text("Index nr: ", font, 20);
    sf::Text indexInputText = sf::Text("", font, 20);
    sf::Text errorText = sf::Text("",calibriFont,20);
    sf::Text errorGoBackText = sf::Text("Go back to registration (ENTER)",font,26);
    sf::Text confirmationText = sf::Text("Are you sure? (y/n): [   ]",calibriFont,25);
    sf::Text confirmEnterText = sf::Text("Confirm (Enter)",calibriFont,25);
    sf::Text confirmInputText = sf::Text("", calibriFont, 25);
    sf::RectangleShape codeLabel = sf::RectangleShape(sf::Vector2f(300, 30));
    sf::Text codeText = sf::Text("Code: ", font, 20);
    sf::Text codeInputText = sf::Text("", font, 20);
    sf::Text cursorIndexPointer = sf::Text("_", font, 20);
    sf::Text cursorCodePointer = sf::Text("_", font, 20);
    sf::Text arrowLinePointer = sf::Text("<",font,20);

public:
    GraphicManager();
    int init_graphic_objects();
    std::string getFormatErrorInfo();
    sf::RectangleShape& getIndexLabel();
    sf::Text& getRegisterText();
    sf::Text& getGuideText();
    sf::Text& getInstructionText1();
    sf::Text& getInstructionText2();
    sf::Text& getInstructionText4();
    sf::Text& getInstructionText5();
    sf::Text& getRequirementsText0();
    sf::Text& getRequirementsText1();
    sf::Text& getIndexText();
    sf::Text& getIndexInputText();
    sf::Text& getErrorText();
    sf::Text& getErrorGoBackText();
    sf::Text& getConfirmationText();
    sf::Text& getConfirmEnterText();
    sf::Text& getConfirmInputText();
    sf::RectangleShape& getCodeLabel();
    sf::Text& getCodeText();
    sf::Text& getCodeInputText();
    sf::Text& getCursorIndexPointer();
    sf::Text& getCursorCodePointer();
    sf::Text& getArrowLinePointer();

};