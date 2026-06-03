# Pedro Olivarez

## Setup

1. Install dotnet sdk

- https://dotnet.microsoft.com/en-us/download/dotnet/10.0

2. Install latest version of Node

- https://nodejs.org/en/download

3. In `./Api` directory

- create a copy of `appsettings.json` and name it `appsettings.Development.json`
- request an invite to neon.tech workspace to get access to connectionstring and paste value in `appsettings.Development.json` file, under `"Neon": {"ConnectionString"}` OR create your own psql set-up with the following schemas and set up a local connection. This is not advised. Just use neon.

```
CREATE SCHEMA "public";

CREATE TABLE "to_dos" (
	"id" integer PRIMARY KEY GENERATED ALWAYS AS IDENTITY (sequence name "to_dos_id_seq" INCREMENT BY 1 MINVALUE 1 MAXVALUE 2147483647 START WITH 1 CACHE 1),
	"title" varchar(500) NOT NULL,
	"body" varchar(1000),
	"created_at" timestamp DEFAULT now() NOT NULL,
	"updated_at" timestamp DEFAULT now() NOT NULL,
	"archived" boolean DEFAULT false NOT NULL,
	"completed" boolean DEFAULT false NOT NULL,
	"user_id" integer NOT NULL
);
ALTER TABLE "to_dos" ENABLE ROW LEVEL SECURITY;

CREATE TABLE "users" (
	"id" integer PRIMARY KEY GENERATED ALWAYS AS IDENTITY (sequence name "users_id_seq" INCREMENT BY 1 MINVALUE 1 MAXVALUE 2147483647 START WITH 1 CACHE 1),
	"email" varchar(500) NOT NULL CONSTRAINT "users_email_key" UNIQUE,
	"password" varchar(500) NOT NULL,
	"created_at" timestamp DEFAULT now() NOT NULL,
	"updated_at" timestamp DEFAULT now() NOT NULL,
	"archived" boolean DEFAULT false NOT NULL,
	"refresh_token" varchar(64) CONSTRAINT "users_refresh_token_key" UNIQUE,
	"refresh_token_expires_at" timestamp
);

CREATE UNIQUE INDEX "to_dos_pkey" ON "to_dos" ("id");
CREATE UNIQUE INDEX "users_email_key" ON "users" ("email");
CREATE UNIQUE INDEX "users_pkey" ON "users" ("id");
CREATE UNIQUE INDEX "users_refresh_token_key" ON "users" ("refresh_token");
```

- generate a super secret string to pop into the "Jwt" : { "Secret" } field as well. Default string is pre-populated but not very secure. For local development it doesn't need to be too super secret, BUT it does need to be decently long otherwise you'll run into this error
  `IDX10720: Unable to create KeyedHashAlgorithm for algorithm 'HS256', the key size must be greater than: '256' bits, key has '<NOT ENOUGH>' bits. (Parameter 'keyBytes')`

4. Install dependencies

- From the root directory, open a terminal and run `npm i`

## API

1. From the root directory
2. Open terminal
3. Run: `dotnet restore`
4. Run: `npm run dev:api`

## Client

1. From the root directory
2. Open terminal
3. Run `npm i`
4. Then run: `npm run dev:ui`

## Considerations / Regrets

1. I did not use Entity framework for some reason. I use EF every day at work and had nostalgic memories of using dapper because I like to "see my sql", OR SO I THOUGHT. I regret not using EF; though it abstracts the queries from you, this can be good because it reduces the need to re-write things and prevents against silly errors like misnamed columns. Alas it is too late for that now ¯\_(ツ)\_/¯
2. I regret not using more third party libraries such as react-hook-form or using google auth. I do not know why I decided to implement auth from scratch. (there's no way to recover a user account if you forget your password because I did not set up an email client or anything of that nature which would probably be necessary for an MVP. If I could go back I would just figure out google auth integration and call it a day)
3. The API structure could benefit from some projects instead of having everything live under API. Not ideal but at this point in the project it's small enough to not really make a difference
4. Everything in `src/components/ui` is just copied over from shadCn save for the `Search` and `Pagination` components. I like this approach because it feels less cluttery than installing an entire UI library via npm for less than a dozen components. Also you get to tinker with the code on a more granular level and perhaps learn from it, so it's highly flexible stuff
   https://ui.shadcn.com/docs
5. Why would anyone decide to implement authentication on their own? That is not a good or fun use of several days
6. No AI was used in the creation of this humble and modest application. I do not pay for any AI subscriptions. I use it at work because it is a tool that helps me do my job. When I am coding for something other than my own financial well-being and survival, I am adamantly in opposition to the ideological project represented by the advancement of AI; and so I do not pay for any subscriptions to any such models. Imagine, if you will, a woodworker who assembles furniture on an assembly line doing some recreational-style woodworking off the clock. I imagine this person doing rugged woodworking with minimal machinery out in the rustic woods and the sun is shining and the animals are chirping and joining along in song. Me writing code is like that, except indoors and connected to wifi and in a gaming chair facing a screen and my back hurts.
7. pls be nice to me
