// WARNING
// This file has been generated automatically by Xamarin Studio to
// mirror C# types. Changes in this file made by drag-connecting
// from the UI designer will be synchronized back to C#, but
// more complex manual changes may not transfer correctly.


#import <UIKit/UIKit.h>
#import <MapKit/MapKit.h>
#import <Foundation/Foundation.h>
#import <CoreGraphics/CoreGraphics.h>


@interface TasksView : UIViewController {
	UITableView *_tblTasks;
	UIView *_headerView;
	UIButton *_btnNuevo;
	UIButton *_btnMap;
}

@property (nonatomic, retain) IBOutlet UITableView *tblTasks;

@property (nonatomic, retain) IBOutlet UIView *headerView;

@property (nonatomic, retain) IBOutlet UIButton *btnNuevo;

@property (nonatomic, retain) IBOutlet UIButton *btnMap;

@end
