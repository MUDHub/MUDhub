import { LogLevel } from '@microsoft/signalr';

export interface IEnviromnent {
	production: boolean;
	api: {
		url: string;
	};
	signalr: {
		url: string;
		logLevel: LogLevel;
	};
}
