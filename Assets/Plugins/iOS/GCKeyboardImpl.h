//import the basics.
#import <Foundation/Foundation.h>
#import <GameController/GameController.h>



//start interface.
@interface GCKeyboardImpl : NSObject  //extend from basic object.

@property ( retain, nonatomic ) GCKeyboard* keyboard;


//define methods.
-(id) init;

-(bool) keyCodeDown: (NSString*) key_code;

//end interface. 
@end
