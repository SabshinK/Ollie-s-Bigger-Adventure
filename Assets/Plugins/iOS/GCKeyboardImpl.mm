//import the header file.
#import "GCKeyboardImpl.h"

//start implementation.
@implementation GCKeyboardImpl

//synthesize variables.
@synthesize keyboard;

typedef GCKeyCode (^KeyCodeCaseBlock)();

NSDictionary *key_code_map = @{
    @"A":
    ^{
        return GCKeyCodeKeyA;
    },
    @"B":
    ^{
        return GCKeyCodeKeyB;
    },
    @"C":
    ^{
        return GCKeyCodeKeyC;
    },
    @"D":
    ^{
        return GCKeyCodeKeyD;
    },
    @"E":
    ^{
        return GCKeyCodeKeyE;
    },
    @"F":
    ^{
        return GCKeyCodeKeyF;
    },
    @"G":
    ^{
        return GCKeyCodeKeyG;
    },
    @"H":
    ^{
        return GCKeyCodeKeyH;
    },
    
    @"I":
    ^{
        return GCKeyCodeKeyI;
    },
    @"J":
    ^{
        return GCKeyCodeKeyJ;
    },
    @"K":
    ^{
        return GCKeyCodeKeyK;
    },
    @"L":
    ^{
        return GCKeyCodeKeyL;
    },
    @"M":
    ^{
        return GCKeyCodeKeyM;
    },
    @"N":
    ^{
        return GCKeyCodeKeyN;
    },
    @"O":
    ^{
        return GCKeyCodeKeyO;
    },
    @"P":
    ^{
        return GCKeyCodeKeyP;
    },
    
    @"Q":
    ^{
        return GCKeyCodeKeyQ;
    },
    @"R":
    ^{
        return GCKeyCodeKeyR;
    },
    @"S":
    ^{
        return GCKeyCodeKeyS;
    },
    @"T":
    ^{
        return GCKeyCodeKeyT;
    },
    @"U":
    ^{
        return GCKeyCodeKeyU;
    },
    @"V":
    ^{
        return GCKeyCodeKeyV;
    },
    @"W":
    ^{
        return GCKeyCodeKeyW;
    },
    @"X":
    ^{
        return GCKeyCodeKeyX;
    },
    
    @"Y":
    ^{
        return GCKeyCodeKeyY;
    },
    @"Z":
    ^{
        return GCKeyCodeKeyZ;
    }
};

//define methods.
-(id) init {
    [[UIApplication sharedApplication]setIdleTimerDisabled:YES];

        // notifications for keyboard (dis)connect
        [[NSNotificationCenter defaultCenter]addObserver:self selector:@selector(keyboardWasConnected:) name:GCKeyboardDidConnectNotification object:nil];
        [[NSNotificationCenter defaultCenter]addObserver:self selector:@selector(keyboardWasDisconnected:) name:GCKeyboardDidDisconnectNotification object:nil];
    
   return self;
}

- (void)keyboardWasConnected:(NSNotification *)notification {
    
    if (!self.keyboard) {
        keyboard = (GCKeyboard *)notification.object;
        NSLog(@"Keyboard connected: %@\n", keyboard.description);
        
        
        self.keyboard = keyboard;
    }
    
}

- (void)keyboardWasDisconnected:(NSNotification *)notification {
    
    if (self.keyboard) {
        GCKeyboard *keyboard = (GCKeyboard *)notification.object;
        NSString *status = [NSString stringWithFormat:@"Keyboard DISCONNECTED:\n%@", keyboard.description];
        NSLog(@"%@\n", status);
        self.keyboard = nil;
    }
}

-(bool) keyCodeDown:(NSString *)key_code {

    KeyCodeCaseBlock lu = key_code_map[key_code];
    bool o = NO;
    
    if (lu) {
        GCKeyCode code = lu();
        GCDeviceButtonInput *button = [GCKeyboard.coalescedKeyboard.keyboardInput buttonForKeyCode:code];

        o = button.pressed;
    }

    
    
    return o;
    
}

//end implementation.
@end

static GCKeyboardImpl *kb = nil;

// Converts C style string to NSString
NSString* CreateNSString (const char* string)
{
    if (string)
        return [NSString stringWithUTF8String: string];
    else
        return [NSString stringWithUTF8String: ""];
}

// Helper method to create C string copy
// Helper method to create C string copy
char* MakeStringCopy (const char* string)
{
    if (string == NULL)
        return NULL;
    
    char* res = (char*)malloc(strlen(string) + 1);
    strcpy(res, string);
    return res;
}

extern "C" {

    const char* _keyCodeDown(const char* code)
    {
        if (kb == nil)
            kb = [[GCKeyboardImpl alloc] init];
        bool o = [kb keyCodeDown:CreateNSString(code)];
        
        return MakeStringCopy (o ? [@"y" UTF8String]:[@"n" UTF8String]);
    }
    
}
