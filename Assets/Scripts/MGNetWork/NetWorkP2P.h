//
//  NetWorkP2P.h
//  P2PTapWar
//
//  Created by  on 11-9-14.
//  Copyright 2011年 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface NetWorkP2P : NSObject
+( NetWorkP2P *) sharedNetWorkP2P;
- (void)findHost;
- (void)createHost;

-(void)testUnityToiOS:(const char *)msg;
-(void)sendMessageToPeer:(const char *)msg;

@end
