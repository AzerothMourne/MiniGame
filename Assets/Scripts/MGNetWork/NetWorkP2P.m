//
//  NetWorkP2P.m
//  P2PTapWar
//
//  Created by  on 11-9-14.
//  Copyright 2011年 __MyCompanyName__. All rights reserved.
//

#import "NetWorkP2P.h"
#import <MultipeerConnectivity/MultipeerConnectivity.h>

// Service name must be < 16 characters
static NSString * const kServiceName = @"multipeer";
static NSString * const kMessageKey = @"message";

@interface NetWorkP2P ()<MCBrowserViewControllerDelegate,MCSessionDelegate>

// Required for both Browser and Advertiser roles
@property (nonatomic, strong) MCPeerID *peerID;
@property (nonatomic, strong) MCSession *session;

// Browser using provided Apple UI
@property (nonatomic, strong) MCBrowserViewController *browserView;

// Advertiser assistant for declaring intent to receive invitations
@property (nonatomic, strong) MCAdvertiserAssistant *advertiserAssistant;

@end

@implementation NetWorkP2P
+(NetWorkP2P *) sharedNetWorkP2P{
    static NetWorkP2P *sharedNetWorkObject;
    if(!sharedNetWorkObject)
        sharedNetWorkObject=[[NetWorkP2P alloc] init];
    return sharedNetWorkObject;
}

- (id)init
{
    self = [super init];
    if (self) {
        // Initialization code here.
    }
    return self;
}
//获取当前屏幕显示的viewcontroller
+ (UIViewController *)getCurrentVC
{
    UIViewController *result = nil;
    
    UIWindow * window = [[UIApplication sharedApplication] keyWindow];
    if (window.windowLevel != UIWindowLevelNormal)
    {
        NSArray *windows = [[UIApplication sharedApplication] windows];
        for(UIWindow * tmpWin in windows)
        {
            if (tmpWin.windowLevel == UIWindowLevelNormal)
            {
                window = tmpWin;
                break;
            }
        }
    }
    
    UIView *frontView = [[window subviews] objectAtIndex:0];
    id nextResponder = [frontView nextResponder];
    
    if ([nextResponder isKindOfClass:[UIViewController class]])
        result = nextResponder;
    else
        result = window.rootViewController;
    
    return result;
}

- (void)findHost{
    _peerID = [[MCPeerID alloc] initWithDisplayName:@"玩家"];
    _session = [[MCSession alloc] initWithPeer:_peerID];
    _session.delegate = self;
    _browserView = [[MCBrowserViewController alloc] initWithServiceType:kServiceName
                                                                session:_session];
    _browserView.delegate = self;
    [[[self class] getCurrentVC] presentViewController:_browserView animated:YES completion:nil];
    
}

- (void)createHost{
    _peerID = [[MCPeerID alloc] initWithDisplayName:@"主机"];
    _session = [[MCSession alloc] initWithPeer:_peerID];
    _session.delegate = self;
    _advertiserAssistant = [[MCAdvertiserAssistant alloc] initWithServiceType:kServiceName
                                                                discoveryInfo:nil
                                                                      session:_session];
    [_advertiserAssistant start];
    
}

-(void)testUnityToiOS:(const char *)msg{
    NSString *str=[NSString stringWithUTF8String:msg];
    NSLog(@"In iOS testUnityToiOS:%@",str);
    str=[NSString stringWithFormat:@"%@+iOSMsg",str];
}
-(void)sendMessageToPeer:(const char *)msg{
    NSString *message = [NSString stringWithUTF8String:msg];
    NSError *error;
    
    [self.session sendData:[message dataUsingEncoding:NSUTF8StringEncoding]
                   toPeers:[_session connectedPeers]
                  withMode:MCSessionSendDataReliable
                     error:&error];
}
#pragma mark - MCBrowserViewControllerDelegate

- (void)browserViewControllerDidFinish:(MCBrowserViewController *)browserViewController {
    [[[self class] getCurrentVC] dismissViewControllerAnimated:YES completion:^{
        [_browserView.browser stopBrowsingForPeers];
    }];
}

- (void)browserViewControllerWasCancelled:(MCBrowserViewController *)browserViewController {
    [[[self class] getCurrentVC] dismissViewControllerAnimated:YES completion:^{
        [_browserView.browser stopBrowsingForPeers];
    }];
}

#pragma mark - MCSessionDelegate

// MCSessionDelegate methods are called on a background queue, if you are going to update UI
// elements you must perform the actions on the main queue.

- (void)session:(MCSession *)session peer:(MCPeerID *)peerID didChangeState:(MCSessionState)state {
    switch (state) {
        case MCSessionStateConnected: {
            dispatch_async(dispatch_get_main_queue(), ^{

            });
            
            // This line only necessary for the advertiser. We want to stop advertising our services
            // to other browsers when we successfully connect to one.
            [_advertiserAssistant stop];
            break;
        }
        case MCSessionStateNotConnected: {
            dispatch_async(dispatch_get_main_queue(), ^{

            });
            break;
        }
        default:
            break;
    }
}

- (void)session:(MCSession *)session didReceiveData:(NSData *)data fromPeer:(MCPeerID *)peerID {
    if ([data length]) {
        dispatch_async(dispatch_get_main_queue(), ^{
            //接受到消息
            unsigned long long nowTimestamp=(unsigned long long)([[NSDate date] timeIntervalSince1970]*1000);
            NSLog(@"%lld",nowTimestamp);
            UnitySendMessage("Main Camera", "receiverMessageFromPeer", [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding].UTF8String);

        });
    }
}

// Required MCSessionDelegate protocol methods but are unused in this application.

- (void)                      session:(MCSession *)session
    didStartReceivingResourceWithName:(NSString *)resourceName
                             fromPeer:(MCPeerID *)peerID
                         withProgress:(NSProgress *)progress {
    
}

- (void)     session:(MCSession *)session
    didReceiveStream:(NSInputStream *)stream
            withName:(NSString *)streamName
            fromPeer:(MCPeerID *)peerID {
    
}

- (void)                       session:(MCSession *)session
    didFinishReceivingResourceWithName:(NSString *)resourceName
                              fromPeer:(MCPeerID *)peerID
                                 atURL:(NSURL *)localURL
                             withError:(NSError *)error {
    
}
@end
