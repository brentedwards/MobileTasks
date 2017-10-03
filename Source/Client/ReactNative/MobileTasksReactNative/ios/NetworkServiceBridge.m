#import <React/RCTBridgeModule.h>
#import <UIKit/UIKit.h>

@interface RCT_EXTERN_MODULE(NetworkService, NSObject)

RCT_EXTERN_METHOD(login:(NSString *)serviceProvider completion:(RCTResponseSenderBlock)callback)

RCT_EXTERN_METHOD(hasPreviousAuthentication:(RCTResponseSenderBlock)callback)

RCT_EXTERN_METHOD(logout:(RCTResponseSenderBlock)callback)

RCT_EXTERN_METHOD(getTasks:(RCTResponseSenderBlock)callback)

RCT_EXTERN_METHOD(upsertTask:(NSString *)task completion:(RCTResponseSenderBlock)callback)

@end
