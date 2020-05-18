import { LogLevel } from '@microsoft/signalr';

export interface IEnviromnent {
	production: boolean;
	resources: {
		base: string;
	};
	api: {
		url: string;
	};
	signalr: {
		url: string;
		logLevel: LogLevel;
	};
}
