var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// ─── HOME PAGE ───────────────────────────────────────────────────────────────
app.MapGet("/", () => Results.Content("""
<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8" />
  <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
  <title>Daniel's API Platform</title>
  <link rel="preconnect" href="https://fonts.googleapis.com">
  <link href="https://fonts.googleapis.com/css2?family=JetBrains+Mono:wght@300;400;700&family=Syne:wght@400;700;800&display=swap" rel="stylesheet">
  <style>
    :root {
      --bg:       #080c10;
      --surface:  #0d1117;
      --border:   #1e2a38;
      --cyan:     #00e5ff;
      --green:    #00ff88;
      --amber:    #ffb830;
      --muted:    #4a5568;
      --text:     #c9d1d9;
      --white:    #f0f6fc;
    }
    *, *::before, *::after { box-sizing: border-box; margin: 0; padding: 0; }

    body {
      background: var(--bg);
      color: var(--text);
      font-family: 'JetBrains Mono', monospace;
      min-height: 100vh;
      overflow-x: hidden;
    }

    /* ── canvas grid ── */
    #grid-canvas {
      position: fixed; inset: 0; z-index: 0;
      opacity: .06; pointer-events: none;
    }

    /* ── NAV ── */
    nav {
      position: sticky; top: 0; z-index: 100;
      display: flex; align-items: center; justify-content: space-between;
      padding: 0 2.5rem; height: 56px;
      background: rgba(8,12,16,.85);
      backdrop-filter: blur(12px);
      border-bottom: 1px solid var(--border);
    }
    .nav-brand {
      display: flex; align-items: center; gap: .6rem;
      font-family: 'Syne', sans-serif; font-weight: 800;
      font-size: 1.05rem; color: var(--white); text-decoration: none;
    }
    .nav-brand .dot { width: 8px; height: 8px; border-radius: 50%;
      background: var(--green); box-shadow: 0 0 8px var(--green);
      animation: pulse 2s infinite; }
    .nav-links { display: flex; gap: 2rem; list-style: none; }
    .nav-links a {
      color: var(--muted); text-decoration: none; font-size: .8rem;
      letter-spacing: .08em; text-transform: uppercase;
      transition: color .2s;
    }
    .nav-links a:hover, .nav-links a.active { color: var(--cyan); }
    .nav-badge {
      font-size: .7rem; padding: 3px 10px; border-radius: 20px;
      background: rgba(0,229,255,.1); color: var(--cyan);
      border: 1px solid rgba(0,229,255,.3);
    }

    /* ── HERO ── */
    .hero {
      position: relative; z-index: 1;
      min-height: calc(100vh - 56px);
      display: flex; flex-direction: column; justify-content: center;
      padding: 6rem 2.5rem 4rem;
      max-width: 1200px; margin: 0 auto;
    }
    .eyebrow {
      font-size: .75rem; letter-spacing: .2em; text-transform: uppercase;
      color: var(--cyan); margin-bottom: 1.5rem;
      display: flex; align-items: center; gap: .8rem;
      opacity: 0; animation: fadeUp .6s .2s forwards;
    }
    .eyebrow::before {
      content: ''; display: block; width: 28px; height: 1px; background: var(--cyan);
    }
    h1 {
      font-family: 'Syne', sans-serif; font-size: clamp(2.8rem, 7vw, 6rem);
      font-weight: 800; line-height: 1.05;
      color: var(--white);
      opacity: 0; animation: fadeUp .6s .35s forwards;
    }
    h1 span { color: var(--cyan); }
    .hero-sub {
      max-width: 560px; margin-top: 1.5rem; font-size: .95rem;
      line-height: 1.8; color: var(--muted);
      opacity: 0; animation: fadeUp .6s .5s forwards;
    }
    .hero-cta {
      display: flex; gap: 1rem; margin-top: 2.5rem; flex-wrap: wrap;
      opacity: 0; animation: fadeUp .6s .65s forwards;
    }
    .btn {
      display: inline-flex; align-items: center; gap: .5rem;
      padding: .75rem 1.75rem; border-radius: 6px;
      font-family: 'JetBrains Mono', monospace; font-size: .82rem;
      font-weight: 700; text-decoration: none; letter-spacing: .04em;
      transition: all .2s; cursor: pointer; border: none;
    }
    .btn-primary {
      background: var(--cyan); color: var(--bg);
    }
    .btn-primary:hover { box-shadow: 0 0 24px rgba(0,229,255,.45); transform: translateY(-1px); }
    .btn-ghost {
      background: transparent; color: var(--text);
      border: 1px solid var(--border);
    }
    .btn-ghost:hover { border-color: var(--cyan); color: var(--cyan); }

    /* ── STATS STRIP ── */
    .stats-strip {
      position: relative; z-index: 1;
      display: grid; grid-template-columns: repeat(auto-fit, minmax(180px, 1fr));
      gap: 1px; background: var(--border);
      border-top: 1px solid var(--border); border-bottom: 1px solid var(--border);
      margin-top: 4rem;
    }
    .stat-cell {
      background: var(--surface); padding: 2.5rem 2rem;
      opacity: 0; animation: fadeUp .6s forwards;
    }
    .stat-cell:nth-child(1){animation-delay:.7s}
    .stat-cell:nth-child(2){animation-delay:.8s}
    .stat-cell:nth-child(3){animation-delay:.9s}
    .stat-cell:nth-child(4){animation-delay:1s}
    .stat-label { font-size: .7rem; letter-spacing: .15em; text-transform: uppercase; color: var(--muted); margin-bottom: .75rem; }
    .stat-value { font-family: 'Syne', sans-serif; font-size: 2.2rem; font-weight: 800; color: var(--white); }
    .stat-value.cyan { color: var(--cyan); }
    .stat-value.green { color: var(--green); }
    .stat-value.amber { color: var(--amber); }

    /* ── ENDPOINTS SECTION ── */
    .section {
      position: relative; z-index: 1;
      max-width: 1200px; margin: 0 auto; padding: 5rem 2.5rem;
    }
    .section-label {
      font-size: .7rem; letter-spacing: .2em; text-transform: uppercase;
      color: var(--cyan); margin-bottom: 1rem;
    }
    .section-title {
      font-family: 'Syne', sans-serif; font-size: 2rem; font-weight: 800;
      color: var(--white); margin-bottom: 3rem;
    }

    .endpoint-list { display: flex; flex-direction: column; gap: 1px; background: var(--border); border: 1px solid var(--border); border-radius: 8px; overflow: hidden; }
    .endpoint-row {
      background: var(--surface); padding: 1.2rem 1.75rem;
      display: flex; align-items: center; gap: 1.25rem;
      transition: background .15s; text-decoration: none;
    }
    .endpoint-row:hover { background: #111820; }
    .method {
      font-size: .7rem; font-weight: 700; letter-spacing: .08em;
      padding: 3px 10px; border-radius: 4px; min-width: 50px; text-align: center;
    }
    .method.get  { background: rgba(0,255,136,.12); color: var(--green); border: 1px solid rgba(0,255,136,.25); }
    .method.post { background: rgba(255,184,48,.12); color: var(--amber); border: 1px solid rgba(255,184,48,.25); }
    .endpoint-path { color: var(--white); font-size: .9rem; flex: 1; }
    .endpoint-desc { color: var(--muted); font-size: .8rem; }
    .arrow { color: var(--border); font-size: .85rem; transition: color .15s, transform .15s; }
    .endpoint-row:hover .arrow { color: var(--cyan); transform: translateX(3px); }

    /* ── TERMINAL BLOCK ── */
    .terminal {
      background: #050810; border: 1px solid var(--border);
      border-radius: 8px; overflow: hidden; margin-top: 3rem;
    }
    .terminal-bar {
      background: #0d1117; padding: .65rem 1rem;
      display: flex; align-items: center; gap: .5rem;
      border-bottom: 1px solid var(--border);
    }
    .term-dot { width: 11px; height: 11px; border-radius: 50%; }
    .term-dot.r { background: #ff5f56; }
    .term-dot.y { background: #ffbd2e; }
    .term-dot.g { background: #27c93f; }
    .term-title { font-size: .75rem; color: var(--muted); margin-left: auto; }
    .terminal-body { padding: 1.5rem 1.75rem; font-size: .82rem; line-height: 2; }
    .line { display: block; }
    .prompt { color: var(--green); }
    .cmd { color: var(--white); }
    .out  { color: var(--muted); }
    .key  { color: var(--cyan); }
    .val  { color: var(--amber); }

    /* ── FOOTER ── */
    footer {
      position: relative; z-index: 1;
      border-top: 1px solid var(--border);
      padding: 2rem 2.5rem;
      display: flex; align-items: center; justify-content: space-between;
      color: var(--muted); font-size: .75rem;
    }
    footer a { color: var(--muted); text-decoration: none; }
    footer a:hover { color: var(--cyan); }

    /* ── ANIMATIONS ── */
    @keyframes fadeUp {
      from { opacity: 0; transform: translateY(18px); }
      to   { opacity: 1; transform: translateY(0); }
    }
    @keyframes pulse {
      0%,100% { opacity: 1; }
      50%      { opacity: .3; }
    }
  </style>
</head>
<body>

<canvas id="grid-canvas"></canvas>

<nav>
  <a class="nav-brand" href="/">
    <span class="dot"></span>
    kss.api
  </a>
  <ul class="nav-links">
    <li><a href="/" class="active">Home</a></li>
    <li><a href="/endpoints">Endpoints</a></li>
    <li><a href="/api/status">API Status</a></li>
    <li><a href="/about">About</a></li>
  </ul>
  <span class="nav-badge">v2.0 · LIVE</span>
</nav>

<main>
  <div class="hero">
    <div class="eyebrow">Azure CI/CD Platform · Production</div>
    <h1>Deploy.<br/><span>Ship.</span><br/>Repeat.</h1>
    <p class="hero-sub">
      A fully automated Web API pipeline built on ASP.NET Core and Azure DevOps.
      From commit to production in seconds — every time.
    </p>
    <div class="hero-cta">
      <a class="btn btn-primary" href="/api/status">→ Check API Status</a>
      <a class="btn btn-ghost" href="/endpoints">View Endpoints</a>
    </div>
  </div>

  <div class="stats-strip">
    <div class="stat-cell">
      <div class="stat-label">Deployment Status</div>
      <div class="stat-value green">LIVE</div>
    </div>
    <div class="stat-cell">
      <div class="stat-label">API Version</div>
      <div class="stat-value cyan">2.0</div>
    </div>
    <div class="stat-cell">
      <div class="stat-label">Platform</div>
      <div class="stat-value">Azure</div>
    </div>
    <div class="stat-cell">
      <div class="stat-label">Pipeline</div>
      <div class="stat-value amber">CI / CD</div>
    </div>
  </div>

  <div class="section">
    <div class="section-label">Endpoints</div>
    <div class="section-title">Available Routes</div>

    <div class="endpoint-list">
      <a class="endpoint-row" href="/">
        <span class="method get">GET</span>
        <span class="endpoint-path">/</span>
        <span class="endpoint-desc">Landing page — this page</span>
        <span class="arrow">›</span>
      </a>
      <a class="endpoint-row" href="/endpoints">
        <span class="method get">GET</span>
        <span class="endpoint-path">/endpoints</span>
        <span class="endpoint-desc">Full endpoint documentation</span>
        <span class="arrow">›</span>
      </a>
      <a class="endpoint-row" href="/api/status">
        <span class="method get">GET</span>
        <span class="endpoint-path">/api/status</span>
        <span class="endpoint-desc">Returns live JSON status payload</span>
        <span class="arrow">›</span>
      </a>
      <a class="endpoint-row" href="/about">
        <span class="method get">GET</span>
        <span class="endpoint-path">/about</span>
        <span class="endpoint-desc">Project info &amp; tech stack</span>
        <span class="arrow">›</span>
      </a>
    </div>

    <div class="terminal">
      <div class="terminal-bar">
        <span class="term-dot r"></span>
        <span class="term-dot y"></span>
        <span class="term-dot g"></span>
        <span class="term-title">bash — production</span>
      </div>
      <div class="terminal-body">
        <span class="line"><span class="prompt">$</span> <span class="cmd">curl -s https://daniel-api.azurewebsites.net/api/status | jq</span></span>
        <span class="line out">{</span>
        <span class="line out">  <span class="key">"status"</span>:   <span class="val">"running"</span>,</span>
        <span class="line out">  <span class="key">"version"</span>:  <span class="val">"2.0"</span>,</span>
        <span class="line out">  <span class="key">"message"</span>:  <span class="val">"Live demo successful!"</span></span>
        <span class="line out">}</span>
        <span class="line" style="margin-top:.5rem"><span class="prompt">$</span> <span class="cmd">_</span></span>
      </div>
    </div>
  </div>
</main>

<footer>
  <span>© 2024 Daniel · Azure CI/CD Demo</span>
  <span>ASP.NET Core · Minimal API · <a href="/api/status">Live Status ↗</a></span>
</footer>

<script>
  // Dot-grid background
  const canvas = document.getElementById('grid-canvas');
  const ctx = canvas.getContext('2d');
  function drawGrid() {
    canvas.width  = window.innerWidth;
    canvas.height = window.innerHeight;
    ctx.clearRect(0, 0, canvas.width, canvas.height);
    ctx.fillStyle = '#00e5ff';
    const gap = 36;
    for (let x = 0; x < canvas.width; x += gap) {
      for (let y = 0; y < canvas.height; y += gap) {
        ctx.beginPath();
        ctx.arc(x, y, 1, 0, Math.PI * 2);
        ctx.fill();
      }
    }
  }
  drawGrid();
  window.addEventListener('resize', drawGrid);
</script>
</body>
</html>
""", "text/html"));

// ─── ENDPOINTS DOC PAGE ───────────────────────────────────────────────────────
app.MapGet("/endpoints", () => Results.Content("""
<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8"/>
  <meta name="viewport" content="width=device-width,initial-scale=1.0"/>
  <title>Endpoints · Daniel's API</title>
  <link href="https://fonts.googleapis.com/css2?family=JetBrains+Mono:wght@300;400;700&family=Syne:wght@400;700;800&display=swap" rel="stylesheet">
  <style>
    :root{--bg:#080c10;--surface:#0d1117;--border:#1e2a38;--cyan:#00e5ff;--green:#00ff88;--amber:#ffb830;--red:#ff5f6d;--muted:#4a5568;--text:#c9d1d9;--white:#f0f6fc;}
    *{box-sizing:border-box;margin:0;padding:0;}
    body{background:var(--bg);color:var(--text);font-family:'JetBrains Mono',monospace;min-height:100vh;}
    nav{position:sticky;top:0;z-index:100;display:flex;align-items:center;justify-content:space-between;padding:0 2.5rem;height:56px;background:rgba(8,12,16,.9);backdrop-filter:blur(12px);border-bottom:1px solid var(--border);}
    .nav-brand{display:flex;align-items:center;gap:.6rem;font-family:'Syne',sans-serif;font-weight:800;font-size:1.05rem;color:var(--white);text-decoration:none;}
    .nav-brand .dot{width:8px;height:8px;border-radius:50%;background:var(--green);box-shadow:0 0 8px var(--green);animation:pulse 2s infinite;}
    .nav-links{display:flex;gap:2rem;list-style:none;}
    .nav-links a{color:var(--muted);text-decoration:none;font-size:.8rem;letter-spacing:.08em;text-transform:uppercase;transition:color .2s;}
    .nav-links a:hover,.nav-links a.active{color:var(--cyan);}
    .nav-badge{font-size:.7rem;padding:3px 10px;border-radius:20px;background:rgba(0,229,255,.1);color:var(--cyan);border:1px solid rgba(0,229,255,.3);}
    .page{max-width:900px;margin:0 auto;padding:4rem 2.5rem 6rem;}
    .page-eyebrow{font-size:.7rem;letter-spacing:.2em;text-transform:uppercase;color:var(--cyan);margin-bottom:1rem;}
    .page-title{font-family:'Syne',sans-serif;font-size:2.8rem;font-weight:800;color:var(--white);margin-bottom:.75rem;}
    .page-sub{color:var(--muted);font-size:.88rem;line-height:1.8;margin-bottom:3.5rem;max-width:520px;}
    .divider{height:1px;background:var(--border);margin:2.5rem 0;}
    .ep-block{margin-bottom:2rem;border:1px solid var(--border);border-radius:8px;overflow:hidden;}
    .ep-header{display:flex;align-items:center;gap:1rem;padding:1.1rem 1.5rem;background:var(--surface);}
    .method{font-size:.7rem;font-weight:700;letter-spacing:.08em;padding:4px 12px;border-radius:4px;min-width:52px;text-align:center;}
    .method.get{background:rgba(0,255,136,.12);color:var(--green);border:1px solid rgba(0,255,136,.25);}
    .ep-path{color:var(--white);font-size:.95rem;flex:1;}
    .ep-tag{font-size:.65rem;padding:2px 8px;border-radius:20px;background:rgba(0,229,255,.1);color:var(--cyan);border:1px solid rgba(0,229,255,.2);}
    .ep-body{padding:1.25rem 1.5rem;border-top:1px solid var(--border);background:#080c10;font-size:.82rem;line-height:1.9;}
    .ep-desc{color:var(--muted);margin-bottom:1rem;}
    .params-title{font-size:.65rem;letter-spacing:.15em;text-transform:uppercase;color:var(--cyan);margin-bottom:.6rem;}
    .param-row{display:flex;gap:1rem;align-items:baseline;margin-bottom:.3rem;}
    .param-name{color:var(--amber);min-width:100px;}
    .param-type{color:var(--muted);font-size:.75rem;min-width:70px;}
    .param-desc{color:var(--text);}
    .response-box{background:var(--surface);border:1px solid var(--border);border-radius:6px;padding:1rem 1.25rem;margin-top:.75rem;}
    .response-box .key{color:var(--cyan);}
    .response-box .val{color:var(--amber);}
    .response-box .muted{color:var(--muted);}
    @keyframes pulse{0%,100%{opacity:1;}50%{opacity:.3;}}
  </style>
</head>
<body>
<nav>
  <a class="nav-brand" href="/"><span class="dot"></span>daniel.api</a>
  <ul class="nav-links">
    <li><a href="/">Home</a></li>
    <li><a href="/endpoints" class="active">Endpoints</a></li>
    <li><a href="/api/status">API Status</a></li>
    <li><a href="/about">About</a></li>
  </ul>
  <span class="nav-badge">v2.0 · LIVE</span>
</nav>

<div class="page">
  <div class="page-eyebrow">Reference</div>
  <h1 class="page-title">API Endpoints</h1>
  <p class="page-sub">All routes are publicly accessible. No authentication required for this demo deployment. Base URL: <span style="color:var(--cyan)">https://daniel-api.azurewebsites.net</span></p>

  <div class="ep-block">
    <div class="ep-header">
      <span class="method get">GET</span>
      <span class="ep-path">/</span>
      <span class="ep-tag">HTML</span>
    </div>
    <div class="ep-body">
      <div class="ep-desc">Returns the main landing page as an HTML document. Entry point for browser visits.</div>
      <div class="params-title">Response · 200 OK</div>
      <div class="response-box">Content-Type: text/html</div>
    </div>
  </div>

  <div class="ep-block">
    <div class="ep-header">
      <span class="method get">GET</span>
      <span class="ep-path">/endpoints</span>
      <span class="ep-tag">HTML</span>
    </div>
    <div class="ep-body">
      <div class="ep-desc">This page — full documentation for all registered routes in the application.</div>
      <div class="params-title">Response · 200 OK</div>
      <div class="response-box">Content-Type: text/html</div>
    </div>
  </div>

  <div class="ep-block">
    <div class="ep-header">
      <span class="method get">GET</span>
      <span class="ep-path">/api/status</span>
      <span class="ep-tag">JSON</span>
    </div>
    <div class="ep-body">
      <div class="ep-desc">Health-check endpoint. Returns a JSON object with the current API runtime status, version, and a demo message. Useful for uptime monitors.</div>
      <div class="params-title">Response · 200 OK</div>
      <div class="response-box">
        {<br/>
        &nbsp;&nbsp;<span class="key">"status"</span>:  <span class="val">"running"</span>,<br/>
        &nbsp;&nbsp;<span class="key">"version"</span>: <span class="val">"2.0"</span>,<br/>
        &nbsp;&nbsp;<span class="key">"message"</span>: <span class="val">"Live demo successful!"</span><br/>
        }
      </div>
    </div>
  </div>

  <div class="ep-block">
    <div class="ep-header">
      <span class="method get">GET</span>
      <span class="ep-path">/about</span>
      <span class="ep-tag">HTML</span>
    </div>
    <div class="ep-body">
      <div class="ep-desc">Project overview, tech stack details, and CI/CD pipeline description.</div>
      <div class="params-title">Response · 200 OK</div>
      <div class="response-box">Content-Type: text/html</div>
    </div>
  </div>
</div>
</body>
</html>
""", "text/html"));

// ─── ABOUT PAGE ───────────────────────────────────────────────────────────────
app.MapGet("/about", () => Results.Content("""
<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8"/>
  <meta name="viewport" content="width=device-width,initial-scale=1.0"/>
  <title>About · Daniel's API</title>
  <link href="https://fonts.googleapis.com/css2?family=JetBrains+Mono:wght@300;400;700&family=Syne:wght@400;700;800&display=swap" rel="stylesheet">
  <style>
    :root{--bg:#080c10;--surface:#0d1117;--border:#1e2a38;--cyan:#00e5ff;--green:#00ff88;--amber:#ffb830;--muted:#4a5568;--text:#c9d1d9;--white:#f0f6fc;}
    *{box-sizing:border-box;margin:0;padding:0;}
    body{background:var(--bg);color:var(--text);font-family:'JetBrains Mono',monospace;min-height:100vh;}
    nav{position:sticky;top:0;z-index:100;display:flex;align-items:center;justify-content:space-between;padding:0 2.5rem;height:56px;background:rgba(8,12,16,.9);backdrop-filter:blur(12px);border-bottom:1px solid var(--border);}
    .nav-brand{display:flex;align-items:center;gap:.6rem;font-family:'Syne',sans-serif;font-weight:800;font-size:1.05rem;color:var(--white);text-decoration:none;}
    .nav-brand .dot{width:8px;height:8px;border-radius:50%;background:var(--green);box-shadow:0 0 8px var(--green);animation:pulse 2s infinite;}
    .nav-links{display:flex;gap:2rem;list-style:none;}
    .nav-links a{color:var(--muted);text-decoration:none;font-size:.8rem;letter-spacing:.08em;text-transform:uppercase;transition:color .2s;}
    .nav-links a:hover,.nav-links a.active{color:var(--cyan);}
    .nav-badge{font-size:.7rem;padding:3px 10px;border-radius:20px;background:rgba(0,229,255,.1);color:var(--cyan);border:1px solid rgba(0,229,255,.3);}
    .page{max-width:900px;margin:0 auto;padding:4rem 2.5rem 6rem;}
    .page-eyebrow{font-size:.7rem;letter-spacing:.2em;text-transform:uppercase;color:var(--cyan);margin-bottom:1rem;}
    .page-title{font-family:'Syne',sans-serif;font-size:2.8rem;font-weight:800;color:var(--white);margin-bottom:.75rem;}
    .page-sub{color:var(--muted);font-size:.88rem;line-height:1.8;margin-bottom:3.5rem;max-width:560px;}
    .grid-2{display:grid;grid-template-columns:1fr 1fr;gap:1px;background:var(--border);border:1px solid var(--border);border-radius:8px;overflow:hidden;margin-bottom:3rem;}
    .grid-cell{background:var(--surface);padding:2rem;}
    .cell-label{font-size:.65rem;letter-spacing:.18em;text-transform:uppercase;color:var(--muted);margin-bottom:.6rem;}
    .cell-value{font-family:'Syne',sans-serif;font-size:1.2rem;font-weight:700;color:var(--white);}
    .cell-value.cyan{color:var(--cyan);}
    .cell-value.green{color:var(--green);}
    .stack-list{display:flex;flex-direction:column;gap:1px;background:var(--border);border:1px solid var(--border);border-radius:8px;overflow:hidden;}
    .stack-row{background:var(--surface);padding:1rem 1.5rem;display:flex;align-items:center;gap:1.25rem;}
    .stack-icon{width:32px;height:32px;border-radius:6px;display:flex;align-items:center;justify-content:center;font-size:.85rem;flex-shrink:0;}
    .stack-name{color:var(--white);font-size:.88rem;flex:1;}
    .stack-desc{color:var(--muted);font-size:.78rem;}
    .pipeline-steps{display:flex;align-items:center;gap:0;margin:3rem 0;flex-wrap:wrap;}
    .pip-step{background:var(--surface);border:1px solid var(--border);border-radius:6px;padding:.75rem 1.25rem;font-size:.78rem;color:var(--text);}
    .pip-step span{display:block;font-size:.6rem;letter-spacing:.1em;text-transform:uppercase;color:var(--muted);margin-bottom:.25rem;}
    .pip-arrow{color:var(--border);padding:0 .75rem;font-size:1.1rem;}
    @keyframes pulse{0%,100%{opacity:1;}50%{opacity:.3;}}
    @media(max-width:600px){.grid-2{grid-template-columns:1fr;}.pipeline-steps{flex-direction:column;align-items:flex-start;}}
  </style>
</head>
<body>
<nav>
  <a class="nav-brand" href="/"><span class="dot"></span>daniel.api</a>
  <ul class="nav-links">
    <li><a href="/">Home</a></li>
    <li><a href="/endpoints">Endpoints</a></li>
    <li><a href="/api/status">API Status</a></li>
    <li><a href="/about" class="active">About</a></li>
  </ul>
  <span class="nav-badge">v2.0 · LIVE</span>
</nav>

<div class="page">
  <div class="page-eyebrow">Project</div>
  <h1 class="page-title">About This API</h1>
  <p class="page-sub">
    A demonstration of a fully automated deployment pipeline using Azure DevOps, ASP.NET Core Minimal APIs, and Azure App Service. Every push to main triggers the CI/CD pipeline and ships to production automatically.
  </p>

  <div class="grid-2">
    <div class="grid-cell">
      <div class="cell-label">Author</div>
      <div class="cell-value">Daniel</div>
    </div>
    <div class="grid-cell">
      <div class="cell-label">Framework</div>
      <div class="cell-value cyan">ASP.NET Core</div>
    </div>
    <div class="grid-cell">
      <div class="cell-label">Hosting</div>
      <div class="cell-value">Azure App Service</div>
    </div>
    <div class="grid-cell">
      <div class="cell-label">Status</div>
      <div class="cell-value green">Live · Production</div>
    </div>
  </div>

  <div class="page-eyebrow" style="margin-bottom:1rem">Tech Stack</div>
  <div class="stack-list" style="margin-bottom:3rem">
    <div class="stack-row">
      <div class="stack-icon" style="background:rgba(0,229,255,.1)">🔷</div>
      <span class="stack-name">ASP.NET Core 8 · Minimal API</span>
      <span class="stack-desc">Web framework</span>
    </div>
    <div class="stack-row">
      <div class="stack-icon" style="background:rgba(0,120,212,.15)">☁️</div>
      <span class="stack-name">Azure App Service</span>
      <span class="stack-desc">Runtime hosting</span>
    </div>
    <div class="stack-row">
      <div class="stack-icon" style="background:rgba(255,184,48,.1)">⚙️</div>
      <span class="stack-name">Azure DevOps Pipelines</span>
      <span class="stack-desc">CI/CD automation</span>
    </div>
    <div class="stack-row">
      <div class="stack-icon" style="background:rgba(0,255,136,.1)">🐙</div>
      <span class="stack-name">GitHub</span>
      <span class="stack-desc">Source control</span>
    </div>
  </div>

  <div class="page-eyebrow" style="margin-bottom:1rem">CI/CD Pipeline</div>
  <div class="pipeline-steps">
    <div class="pip-step"><span>Step 1</span>git push</div>
    <div class="pip-arrow">→</div>
    <div class="pip-step"><span>Step 2</span>Build</div>
    <div class="pip-arrow">→</div>
    <div class="pip-step"><span>Step 3</span>Test</div>
    <div class="pip-arrow">→</div>
    <div class="pip-step"><span>Step 4</span>Publish</div>
    <div class="pip-arrow">→</div>
    <div class="pip-step" style="border-color:var(--green);color:var(--green)"><span style="color:var(--muted)">Step 5</span>🚀 Deploy</div>
  </div>
</div>
</body>
</html>
""", "text/html"));

// ─── JSON API ─────────────────────────────────────────────────────────────────
app.MapGet("/api/status", () =>
    Results.Json(new
    {
        status = "running",
        version = "2.0",
        message = "Live demo successful!",
        uptime = "online",
        platform = "Azure App Service",
        pipeline = "Azure DevOps CI/CD"
    }));

app.Run();