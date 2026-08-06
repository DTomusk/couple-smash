import { Stack, Typography } from "@mui/material";

export default function GonePage() {
    return (
        <Stack spacing={2} sx={{ padding: 1, overflowY: 'auto' }}>
            <Typography variant="h1">Gone</Typography>
            <Typography variant="body1">CoupleSmash is gone for good. Thank you for coming along for the ride.</Typography>
        </Stack>
    )
}