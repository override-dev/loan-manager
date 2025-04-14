import {
    type RouteConfig,
    route,
    index,
} from "@react-router/dev/routes";

export default [

    route("home", "./routes/home.tsx", [
        index("./routes/overview.tsx"),
    ]),
] satisfies RouteConfig;