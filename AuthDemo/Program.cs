using AuthDemo;


// The demo is about QA-led automation and CI gates, not building a full UI.

var store = new InMemoryUserStore();
var auth = new AuthService(store);

Console.WriteLine("QA CI demo: authentication policy enforced by tests and CI gates.");
Console.WriteLine($"Lockout threshold is set to: {AuthService.LockoutThreshold} failed attempts.");
