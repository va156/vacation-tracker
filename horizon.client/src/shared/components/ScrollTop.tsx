import Box from "@mui/material/Box";
import Fade from "@mui/material/Fade";
import useScrollTrigger from "@mui/material/useScrollTrigger";
import * as React from "react";

interface Props {
	window?: () => Window;
	children?: React.ReactElement<unknown>;
}

export default function ScrollTop(props: Props) {
	const { children } = props;
	const trigger = useScrollTrigger({
		disableHysteresis: true,
		threshold: 100,
	});

	const handleClick = (event: React.MouseEvent<HTMLDivElement>) => {
		const anchor = (
			(event.target as HTMLDivElement).ownerDocument || document
		).querySelector("#back-to-top-anchor");

		if (anchor) {
			anchor.scrollIntoView({
				block: "center",
			});
		}
	};

	return (
		<Fade in={trigger}>
			<Box
				onClick={handleClick}
				role="presentation"
				sx={{ position: "fixed", bottom: 16, right: 16 }}
			>
				{children}
			</Box>
		</Fade>
	);
}
