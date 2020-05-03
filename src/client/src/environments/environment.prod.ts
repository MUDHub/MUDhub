import { IEnviromnent } from './IEnvironment';
import { LogLevel } from '@microsoft/signalr';

export const environment: IEnviromnent = {
	production: true,
	api: {
		url: ''
	},
	signalr: {
		url: '',
		logLevel: LogLevel.Error
	}
};
