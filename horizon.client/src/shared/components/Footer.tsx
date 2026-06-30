import { Box, Container, Typography } from "@mui/material";
import { FunctionComponent } from "react";

const Footer: FunctionComponent = () => {
    return (
        <Box sx={{ py: 3, bgcolor: '#f8f9fa', borderTop: '1px solid #eee' }}>
            <Container maxWidth="lg">
                <Typography textAlign="center" variant="body2" color="text.secondary">
                    © {new Date().getFullYear()} Horizon
                </Typography>
            </Container>
        </Box>
    );
};

export default Footer;