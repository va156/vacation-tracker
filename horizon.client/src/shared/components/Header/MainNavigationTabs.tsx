import { Box, Tab, Tabs } from "@mui/material";
import { matchPath, NavLink, useLocation } from "react-router-dom";

function useRouteMatch(patterns: readonly string[]) {
    const { pathname } = useLocation();

    for (let i = 0; i < patterns.length; i += 1) {
        const pattern = patterns[i];
        const possibleMatch = matchPath(pattern, pathname);
        if (possibleMatch !== null) {
            return possibleMatch;
        }
    }

    return null;
}

export default function MainNavigationTabs() {
    const routeMatch = useRouteMatch(["/dashboard", "/employees", "/requests"]);
    const currentTab = routeMatch?.pattern?.path;

    return (
        <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
            <Tabs
                value={currentTab || "/dashboard"}
                textColor="primary"
                indicatorColor="primary"
                sx={{
                    '& .MuiTab-root': {
                        textTransform: 'none',
                        fontWeight: 500,
                        fontSize: '0.95rem',
                        minHeight: 48,
                        px: 3,
                    }
                }}
            >
                <Tab
                    value="/dashboard"
                    label="Мои отпуска"
                    component={NavLink}
                    to="/dashboard"
                />
                <Tab
                    value="/employees"
                    label="Сотрудники"
                    component={NavLink}
                    to="/employees"
                />
                <Tab
                    value="/requests"
                    label="Заявки"
                    component={NavLink}
                    to="/requests"
                />
            </Tabs>
        </Box>
    );
}