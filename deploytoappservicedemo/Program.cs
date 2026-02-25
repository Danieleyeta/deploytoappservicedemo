var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// 1. A nice visual landing page for the root URL (/)
app.MapGet("/", () => Results.Content(@"
    <!DOCTYPE html>
    <html>
    <head>
        <title>API Demo</title>
        <style>
            body { 
                font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; 
                background: linear-gradient(135deg, #0078D4 0%, #00bcda 100%); 
                color: #333; 
                display: flex; 
                justify-content: center; 
                align-items: center; 
                height: 100vh; 
                margin: 0; 
            }
            .card { 
                background-color: white; 
                padding: 40px 50px; 
                border-radius: 12px; 
                box-shadow: 0 10px 20px rgba(0,0,0,0.2); 
                text-align: center; 
            }
            h1 { color: #0078D4; margin-top: 0; }
            .badge { 
                background-color: #28a745; 
                color: white; 
                padding: 6px 12px; 
                border-radius: 20px; 
                font-size: 0.9em; 
                font-weight: bold;
                display: inline-block;
                margin: 10px 0;
            }
            a { color: #0078D4; text-decoration: none; font-weight: bold; }
            a:hover { text-decoration: underline; }
        </style>
    </head>
    <body>
        <div class='card'>
            <h1>🚀 Azure CI/CD Demo</h1>
            <h2> Successfully Deployed!</h2>
            <h1>🚀 Azure CI/CD Demo</h1>
            <h2> Successfully Deployed new!</h2>
            <p>Welcome to Daniel's automated Web API!</p>
            <div class='badge'>Status: Live in Production</div>
            <p style='margin-top: 25px; font-size: 0.9em; color: #666;'>
                View the raw JSON data: <br/>
                👉 <a href='/api/status'>/api/status</a>
            </p>
        </div>
    </body>
    </html>
", "text/html"));

// 2. The original API endpoint returning JSON data
app.MapGet("/api/status", () =>
    new { status = "running", version = "2.0", message = "Live demo successful!" });

app.Run();