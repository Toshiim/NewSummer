## AI News Summarizer
A personal digest service with LLM-based categorization, summarization, and scoring
–	Domain model and EF Core data layer
–	REST API: GET /articles (pagination, filtering, sorting), GET/POST /sources — new sources added dynamically (tested on BBC, Habr, DTF)
–	RSS feed parsing and article crawling via SmartReader, working with any added source
–	Summarization, categorization, and scoring of articles via LLM, orchestrated as background jobs via Hangfire
–	Fully containerized deployment (docker-compose up), with Nvidia GPU support for local LLM inference
–	Platform-agnostic bot architecture — adding a new bot only requires a Handler and a Sender
–	Telegram bot implemented with commands: /start, /topics, /mysubs, /subscribe, /unsubscribe, /latest
–	Personalized digest generation based on user-selected categories, factoring in news importance and urgency
