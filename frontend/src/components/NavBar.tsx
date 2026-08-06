import { Typography } from '@mui/material';
import AppBar from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';

export default function NavBar() {
    return (
        <>
            <AppBar position="fixed" color="primary">
                <Toolbar sx={{ display: 'flex', justifyContent: 'space-between' }}>
                    <Typography variant="h5">💍Couple Smash💑</Typography>
                    {/* <Button variant="text" color="inherit" onClick={() => navigate('/optimal-pairings')}>
                        Optimal Pairings
                    </Button> */}
                </Toolbar>
            </AppBar>
        </>
    )
}