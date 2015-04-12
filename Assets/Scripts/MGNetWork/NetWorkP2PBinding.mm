#import "NetWorkP2P.h"

extern "C"{
    void _findHost()
    {
        [[NetWorkP2P sharedNetWorkP2P] findHost];
        NSLog(@"call the mothed _findHost");
    }
    void _createHost()
    {
        [[NetWorkP2P sharedNetWorkP2P] createHost];
        NSLog(@"call the mothed _createHost");
    }
    void _testUnityToiOS(const char *msg) {
        return [[NetWorkP2P sharedNetWorkP2P] testUnityToiOS:msg];
    }
    void _sendMessageToPeer(const char *msg) {
        return [[NetWorkP2P sharedNetWorkP2P] sendMessageToPeer:msg];
    }
}
