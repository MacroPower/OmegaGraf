declare module 'rc-steps' {
    export default class Steps extends React.Component<{current: number, direction?: string, size?: string, icons?: any}> {
        static Step = class Step extends React.Component<{title: string, description: string, icon?: any, ref?: any}> {}
    }
}